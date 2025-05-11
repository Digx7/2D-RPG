using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEditor;
using UnityEditor.Localization;
using System.Collections.ObjectModel;

public class LocTableEditor : Editor
{
    [MenuItem("Assets/Create/Table Entry")]
    public static void CreateTableEntry()
    {
        StringTableCollection stringTableCollection = (StringTableCollection) AssetDatabase.LoadAssetAtPath("Assets/MyTable.asset", typeof(StringTableCollection));

        Locale englishLocal = (Locale) AssetDatabase.LoadAssetAtPath("Assets/English (en).asset", typeof(Locale));

        if(englishLocal != null)
        {
            Debug.Log("English Locale Found");
        }

        if(stringTableCollection != null)
        {
            SharedTableData sharedTableData = stringTableCollection.SharedData;
            if(sharedTableData.Contains("Entry 1")) Debug.Log("Entry 1  Found");
            if(!sharedTableData.Contains("Entry 3")) sharedTableData.AddKey("Entry 3");
            if(sharedTableData.Contains("Entry 3")) Debug.Log("Entry 3  Found");

            ReadOnlyCollection<StringTable> stringTables = stringTableCollection.StringTables;

            for(int i = 0; i < stringTables.Count; ++i)
            {
                if(stringTables[i].LocaleIdentifier == englishLocal.Identifier)
                {
                    Debug.Log("English Table Found");
                    stringTables[i].AddEntry("Entry 3", "Update");
                }
            }
        }
    }
}
