using System.Linq;
using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{
    using UnityEditor;
    
    [CustomEditor(typeof(SaveFile)), CanEditMultipleObjects]
    public class SaveFileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector first
            base.OnInspectorGUI();

            //
            // Extra Editor Tools
            //
        
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
        
            var saveFiles = targets.OfType<SaveFile>();
        
            // Clear Button
            if (GUILayout.Button("Clear Save Data"))
            {
                foreach (var saveFile in saveFiles)
                {
                    saveFile.ClearSave();
                }
            }
        }

        public static void ClearButton(SaveFile saveFile)
        {
            if (GUILayout.Button("Clear Save Data"))
            {
                saveFile.ClearSave();
            }
        }
    }
}


