using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class DialogueCreator : Editor
{
    [MenuItem("Assets/Create/Dialogue")]
    public static void CreateDialogue()
    {
        Conversation conversation = (Conversation) AssetDatabase.LoadAssetAtPath("Assets/NewConversation.asset", typeof(Conversation));

        if(conversation == null)
        {
            conversation = ScriptableObject.CreateInstance<Conversation>();

            conversation.nodes = new List<ConversationNode>();

            ConversationNode_Line m_line = new ConversationNode_Line();
            m_line.ID = "1";
            // m_line.UID = m_line.ID;
            m_line.nextNode = m_line.ID + "1";
            m_line.speaker = "Dev";
            m_line.line = "Hello World!";

            conversation.nodes.Add ( m_line );

            AssetDatabase.CreateAsset(conversation, "Assets/NewConversation.asset");
            AssetDatabase.SaveAssets();
        }
        else
        {
            ConversationNode_Line m_line = new ConversationNode_Line();
            // m_line.ID = conversation.nodes.Count;
            // m_line.UID = m_line.ID;
            m_line.nextNode = m_line.ID + "1";
            m_line.speaker = "Dev";
            m_line.line = "New Line!";

            conversation.nodes.Add ( m_line );
        }

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = conversation;
    }
}
