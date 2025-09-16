using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(SaveManager))]
    public class SaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector first
            base.OnInspectorGUI();

            //
            // Extra Editor Tools
            //

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save Tools", EditorStyles.boldLabel);

            SaveManager manager = (SaveManager)target;

            ShowTools(manager);
        }

        public static void ShowTools(SaveManager manager)
        {
            SaveFile saveFile = manager?.settings?.saveFile;

            if (saveFile)
            {
                SaveButton(manager);
                LoadButton(manager);
                LoadNoRestoreButton(manager);

                SaveFileEditor.ClearButton(saveFile);
            }
            else
            {
                EditorGUILayout.HelpBox("Assign Save File for Tools", MessageType.Error);
            }

        }

        public static void SaveButton(SaveManager manager)
        {
            if (GUILayout.Button("Save Data"))
            {
                if (manager.SaveData() && manager.settings.showDebug)
                    Debug.Log("Data saved.");
            }
        }

        public static void LoadButton(SaveManager manager)
        {
            if (GUILayout.Button("Load Data"))
            {
                if (manager.LoadData() && manager.settings.showDebug)
                    Debug.Log("Data Loaded and Restored to objects.");
            }
        }

        public static void LoadNoRestoreButton(SaveManager manager)
        {
            if (GUILayout.Button("Load Data (No Restore)"))
            {
                if (manager.LoadData(restore: false) && manager.settings.showDebug)
                    Debug.Log("Data loaded.");
            }
        }
    }
}