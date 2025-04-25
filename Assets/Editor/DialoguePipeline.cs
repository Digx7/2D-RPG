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

public class DialoguePipeline : Editor
{
    const string PATH_BASE = "Assets/Dialogue/";
    const string STRING_TABLE_PATH = "Assets/Localization/Tables/Dialogue.asset";
    const string LOCALE_PATH = "Assets/Localization/Locals/English (en).asset";
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
        }
        
    }

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
            if(passage.tags == "Line")
            {
                ConversationNode_Line newLine = new ConversationNode_Line();

                newLine.ID = passage.name;
                newLine.LocKey = name + "_" + passage.name;
                newLine.nextNode = passage.links[0].passageName;

                newLine.line = ParseLineText(passage.cleanText, out newLine.speaker);


                conversation.nodes.Add(newLine);
            }
            else if (passage.tags == "Options")
            {
                ConversationNode_Options newOptions = new ConversationNode_Options();
                newOptions.options = new List<OptionPair>();
                newOptions.LocKey = name + "_" + passage.name;

                newOptions.ID = passage.name;

                for (int i = 0; i < passage.links.Count; i++)
                {
                    OptionPair xOption = new OptionPair();
                    xOption.nextNode = passage.links[i].passageName;
                    newOptions.options.Add(xOption);
                }

                string[] subStrings = passage.cleanText.Split('\n');
                int j = 0;
                for (int i = 0; i < subStrings.Length; i++)
                {
                    if(subStrings[i] == "") continue;

                    if(subStrings[i].Contains("//Text: "))
                    {
                        string line = subStrings[i];
                        line = line.Remove(0, 8);

                        OptionPair jOption = newOptions.options[j];
                        jOption.line = line;

                        newOptions.options[j] = jOption;

                        j++;
                    }
                }

                conversation.nodes.Add(newOptions);
            }
            else if (passage.tags == "QuestUpdate")
            {
                ConversationNode_QuestUpdate newQuestUpdate = new ConversationNode_QuestUpdate();

                newQuestUpdate.ID = passage.name;
                newQuestUpdate.nextNode = passage.links[0].passageName;

                conversation.nodes.Add(newQuestUpdate);
            }
            else if (passage.tags == "End")
            {
                ConversationNode_End newEnd = new ConversationNode_End();

                newEnd.ID = passage.name;

                conversation.nodes.Add(newEnd);
            }
        }
    }

    public static string ParseLineText(string input, out string speaker)
    {
        string cleanLine = "";
        speaker = "???";

        string[] subStrings = input.Split('\n');

        bool buildingLine = false;
        for(int i = 0; i < subStrings.Length; ++i)
        {
            if(buildingLine)
            {
                cleanLine = cleanLine + subStrings[i] + "\n";
            }
            else
            {
                if(subStrings[i].Contains("//Speaker: "))
                {
                    speaker = subStrings[i];
                    speaker = speaker.Remove(0, 11);
                }
                else if(subStrings[i].Contains("//Body:"))
                {
                    buildingLine = true;
                }
            }
        }

        return cleanLine;
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
