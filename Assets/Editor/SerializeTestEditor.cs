using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SerializeReferencePolymorphismExample))]
public class SerializeTestEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector ();
        SerializeReferencePolymorphismExample myTarget = (SerializeReferencePolymorphismExample) target;

        if(GUILayout.Button("Add Apple"))
        {
            myTarget.m_Items.Add ( new Apple() );
        }
        if(GUILayout.Button("Add Orange"))
        {
            myTarget.m_Items.Add ( new Orange() );
        }

    }
}
