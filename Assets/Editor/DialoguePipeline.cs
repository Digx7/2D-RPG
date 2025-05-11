using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEditor;
using UnityEditor.Localization;
using Unity.EditorCoroutines.Editor;

public class DialoguePipeline : Editor
{
    const string PATH_BASE = "Assets/Dialogue/";
    const string STRING_TABLE_PATH = "Assets/Localization/Tables/Dialogue.asset";
    const string LOCALE_PATH = "Assets/Localization/Locals/English (en).asset";
    const string ICON_PATH_BASE = "Assets/Dialogue/Icons/";
    const string QUEST_PATH_BASE = "Assets/Quests/Resources/";
    static string name;
    
    static TextAsset jsonAsset;
    static TwineStory twineStory;
    static Conversation conversation;
    static StringTableCollection stringTableCollection;
    static StringTable englishTable;
    static Locale englishLocale;
    
    [MenuItem("Assets/DialoguePipeline")]
    public static void Pipeline()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            if(!(Selection.objects[i] is TextAsset)) continue;
            
            // Json -> TwineStory ========
            SetupJsonAsset(Selection.objects[i]);

            JsonToTwineStory();



            // TwineStory -> Conversation ========
            TryToGetConversationAsset();
            if(conversation == null)
            {
                // Create new conversation
                conversation = ScriptableObject.CreateInstance<Conversation>();

                conversation.nodes = new List<ConversationNode>();

                TwineStoryPassagesToConversationNodes();

                AssetDatabase.CreateAsset(conversation, GetConversationPath());
            }
            else
            {
                // Modify existing conversation
                conversation.nodes.Clear();

                TwineStoryPassagesToConversationNodes();
            }

            // Conversation -> LocTables ===============
            TryToGetStringTable();
            TryToGetLocale();
            TryToGetEnglishTable();

            EditLocalizationTables();


            // Cleanup
            TryToDeleteTwineStory();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        
    }

    // [MenuItem("Assets/DialoguePipeline")]
    // public static void StartFlow()
    // {
    //     EditorCoroutineUtility.StartCoroutineOwnerless(Flow());
    // }

    // static IEnumerator Flow()
    // {
    //     Pipeline();
    //     yield return new WaitForSeconds(0.5f);
    //     Pipeline();
    // }

    public static void SetupJsonAsset(Object obj)
    {
        // jsonAsset = (TextAsset) Selection.activeObject;
        jsonAsset = (TextAsset) obj;
    }

    public static void JsonToTwineStory()
    {
        twineStory = ScriptableObject.CreateInstance<TwineStory>();
        JsonUtility.FromJsonOverwrite(jsonAsset.text, twineStory);
        name = twineStory.name;

        AssetDatabase.CreateAsset(twineStory, GetTwineStoryPath());
        AssetDatabase.SaveAssets();

    }

    public static void TwineStoryPassagesToConversationNodes()
    {
        foreach (TwinePassage passage in twineStory.passages)
        {
            // Gets all components
            Dictionary<string, string> components;
            ParseLineText(passage.cleanText, out components);
            
            if(passage.tags == "Line")
            {
                ConversationNode_Line newLine = new ConversationNode_Line();

                newLine.ID = passage.name;
                newLine.LocKey = name + "_" + passage.name;
                newLine.nextNode = passage.links[0].passageName;

                // Gets Body, Speaker, Icon
                if(components.ContainsKey("//Body:")) newLine.line = components["//Body:"];
                if(components.ContainsKey("//Speaker:"))newLine.speaker = components["//Speaker:"];
                if(components.ContainsKey("//Icon:"))
                {
                    Sprite newIcon = (Sprite) AssetDatabase.LoadAssetAtPath(ICON_PATH_BASE + components["//Icon:"] + ".png", typeof(Sprite));
                    newLine.icon = newIcon;
                }


                conversation.nodes.Add(newLine);
            }
            else if (passage.tags == "Options")
            {
                ConversationNode_Options newOptions = new ConversationNode_Options();

                newOptions.ID = passage.name;
                newOptions.LocKey = name + "_" + passage.name;
                newOptions.options = new List<OptionPair>();

                // Gets Body, Speaker, Icon
                if(components.ContainsKey("//Body:")) newOptions.line = components["//Body:"];
                if(components.ContainsKey("//Speaker:"))newOptions.speaker = components["//Speaker:"];
                if(components.ContainsKey("//Icon:"))
                {
                    Sprite newIcon = (Sprite) AssetDatabase.LoadAssetAtPath(ICON_PATH_BASE + components["//Icon:"] + ".png", typeof(Sprite));
                    newOptions.icon = newIcon;
                }

                // Gets options
                for (int i = 0; i < passage.links.Count; i++)
                {
                    OptionPair xOption = new OptionPair();
                    
                    xOption.nextNode = passage.links[i].passageName;

                    string key = "//Option_" + (i + 1) + ":";
                    if(components.ContainsKey(key))
                    {
                        xOption.line = components[key]; 
                    }

                    newOptions.options.Add(xOption);
                }

                conversation.nodes.Add(newOptions);
            }
            else if (passage.tags == "QuestUpdate")
            {
                ConversationNode_QuestUpdate newQuestUpdate = new ConversationNode_QuestUpdate();

                newQuestUpdate.ID = passage.name;
                newQuestUpdate.nextNode = passage.links[0].passageName;

                if(components.ContainsKey("//QuestName:"))
                {
                    newQuestUpdate.questToGive = (QuestData) AssetDatabase.LoadAssetAtPath(QUEST_PATH_BASE + components["//QuestName:"] + ".asset", typeof(QuestData));
                }

                if(components.ContainsKey("//QuestObjectiveName:"))
                {
                    newQuestUpdate.questProgressToGive.objectiveName = components["//QuestObjectiveName:"];
                }

                conversation.nodes.Add(newQuestUpdate);
            }
            else if (passage.tags == "End")
            {
                ConversationNode_End newEnd = new ConversationNode_End();

                newEnd.ID = passage.name;

                if(components.ContainsKey("//NextConversation:"))
                {
                    newEnd.nextConversationToLoadOnFinish = (Conversation) AssetDatabase.LoadAssetAtPath(PATH_BASE + components["//NextConversation:"] + "_Conversation.asset", typeof(Conversation));
                }

                conversation.nodes.Add(newEnd);
            }
        }
    }

    public static void ParseLineText(string input, out Dictionary<string, string> components)
    {
        components = new Dictionary<string, string>();

        string[] subStrings = input.Split('\n');

        string key = "";
        string value = "";

        bool buildingLine = false;
        for(int i = 0; i < subStrings.Length; ++i)
        {
            if(buildingLine)
            {
                value += subStrings[i];
            }

            if(subStrings[i].Contains("//"))
            {
                string[] jSubStrings = subStrings[i].Split(' ');
                key = jSubStrings[0];

                if (jSubStrings.Length >= 2)
                {
                    // value = jSubStrings[1];
                    value = "";
                    for (int j = 1; j < jSubStrings.Length; j++)
                    {
                        value += jSubStrings[j];

                        // Only adds a space if length is greater than 2 (the first indext is always the key)
                        // Also does NOT add a space if we are on the last value
                        if(jSubStrings.Length > 2 && j < (jSubStrings.Length - 1)) value += " ";
                    }
                } 
                    

                if(key == "//Body:")
                {
                    buildingLine = true;
                    value = "";
                }
                else if(key == "//End_Body:")
                {
                    key = "//Body:";
                    value = value.Remove(value.Length - 11);
                    components[key] = value;
                }
                else
                {
                    components[key] = value;
                }
            }
        }
    }

    public static void EditLocalizationTables()
    {
        if(stringTableCollection == null)
        {
            Debug.LogError("Dialogue Pipeline: Tried to edit localization tables while StringTableCollection is undefined");
        }
        
        SharedTableData sharedTableData = stringTableCollection.SharedData;

        if(sharedTableData == null)
        {
            Debug.LogError("Dialogue Pipeline: Failed to find sharedTableData");
        }

        for (int i = 0; i < conversation.nodes.Count; i++)
        {
            if(!(conversation.nodes[i] is ConversationNode_Line)) continue;

            ConversationNode_Line currentLine = (ConversationNode_Line) conversation.nodes[i];
            string key = currentLine.LocKey;
            string line = currentLine.line;
            
            if(!sharedTableData.Contains(key))
            {
                // Create new entry
                sharedTableData.AddKey(key);
            }
            // Set english entry
            englishTable.AddEntry(key, line);
        }
    }

    public static void TryToGetConversationAsset()
    {
        conversation = (Conversation) AssetDatabase.LoadAssetAtPath(GetConversationPath(), typeof(Conversation));
    }

    public static void TryToGetStringTable()
    {
        stringTableCollection = (StringTableCollection) AssetDatabase.LoadAssetAtPath(STRING_TABLE_PATH, typeof(StringTableCollection));
    
        if(stringTableCollection == null)
        {
            Debug.LogError("DialoguePipeLine: No String Table exists at " + STRING_TABLE_PATH);
        }
    }

    public static void TryToGetEnglishTable()
    {
        if(stringTableCollection == null)
        {
            Debug.LogError("DialoguePipeline: Tried to get english table while StringTableCollection is undefined");
        }
        
        ReadOnlyCollection<StringTable> stringTables = stringTableCollection.StringTables;
        
        for(int i = 0; i < stringTables.Count; ++i)
        {
            if(stringTables[i].LocaleIdentifier == englishLocale.Identifier)
            {
                englishTable = stringTables[i];
                break;
            }
        }

        if(englishTable == null)
        {
            Debug.LogError("DialoguePipeline: Failed to find english table");
        }
    }

    public static void TryToGetLocale()
    {
        englishLocale = (Locale) AssetDatabase.LoadAssetAtPath(LOCALE_PATH, typeof(Locale));

        if(englishLocale == null)
        {
            Debug.LogError("DialoguePipeLine: No Locale exists at " + LOCALE_PATH);
        }
    }

    public static void TryToDeleteTwineStory()
    {
        AssetDatabase.DeleteAsset(GetTwineStoryPath());
    }

    public static string GetConversationPath()
    {
        if(name == "")
        {
            Debug.LogError("DialoguePipeline: Failed to get a valid Conversation path because Name was undefined");
        }
        
        return PATH_BASE + name + "_Conversation.asset";
    }

    public static string GetTwineStoryPath()
    {
        if(name == "")
        {
            Debug.LogError("DialoguePipeline: Failed to get a valid Twine Story path because Name was undefined");
        }
        
        return PATH_BASE + name + "_TwineStory.asset";
    }
}
