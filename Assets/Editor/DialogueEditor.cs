using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Conversation))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();
        Conversation myTarget = (Conversation) target;

        if(GUILayout.Button("Add Line"))
        {
            myTarget.nodes.Add ( new ConversationNode_Line() );
        }
        if(GUILayout.Button("Add Options"))
        {
            myTarget.nodes.Add ( new ConversationNode_Options() );
        }
        if(GUILayout.Button("Add QuestUpdate"))
        {
            myTarget.nodes.Add ( new ConversationNode_QuestUpdate() );
        }
        if(GUILayout.Button("Add End"))
        {
            myTarget.nodes.Add ( new ConversationNode_End() );
        }

    }
}
