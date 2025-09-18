using System.Linq;
using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{
    using UnityEditor;
    
    [CustomEditor(typeof(SaveFile)), CanEditMultipleObjects]
    internal sealed class SaveFileEditor : Editor
    {
        private static GUIContent ClearAllContent => 
            new GUIContent("Clear All Save Data", "Clears all file data, Including Temp save data. Will not alter the currently playing scene or loaded data.");

        private static GUIContent ClearTempContent =>
            new GUIContent("Clear Temp Save Data", "Clears the temp save data. Will not alter the currently playing scene or loaded data.");
        
        public override void OnInspectorGUI()
        {
            // Draw the default inspector first
            base.OnInspectorGUI();

            //
            // Extra Editor Tools
            //
        
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
        
            var saveFiles = targets.OfType<SaveFile>().ToList();
        
            // Clear all Button
            if (GUILayout.Button(ClearAllContent))
            {
                foreach (var saveFile in saveFiles)
                {
                    saveFile.ClearSave();
                }
            }
            
            // Clear temp button
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(ClearTempContent))
            {
                foreach (var saveFile in saveFiles)
                {
                    saveFile.ClearTempSave();
                }
            }
            GUI.enabled = true;
        }

        public static void ClearButtons(SaveFile saveFile, bool showLogs=true)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Save File Tools");
            
            // Clear all Button
            if (GUILayout.Button(ClearAllContent))
            {
                saveFile.ClearSave(showLogs);
            }
            
            //  temp button
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button(ClearTempContent))
            {
                saveFile.ClearTempSave(showLogs);
            }
            GUI.enabled = true;
        }
    }
}


