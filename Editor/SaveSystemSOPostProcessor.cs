using UnityEditor;
using UnityEngine;

namespace WolverineSoft.SaveSystem.Editor
{
    /// <summary>
    /// Post-processor for SO assets which is used to assign the proper icons
    /// </summary>
    [InitializeOnLoad]
    internal static class SaveSystemSOPostProcessor
    {
        private static Texture2D saveFileIcon => Resources.Load<Texture2D>("SaveFileIcon");
        private static Texture2D saveSystemSettingIcon => Resources.Load<Texture2D>("SaveSystemSettingsIcon");

        static SaveSystemSOPostProcessor()
        {
            // Runs when the editor loads
            EditorApplication.projectChanged += AssignAllIcons;
            AssignAllIcons();
        }

        private static void AssignAllIcons()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (obj == null) continue;

                if (obj is SaveFile && saveFileIcon != null)
                    EditorGUIUtility.SetIconForObject(obj, saveFileIcon);

                else if (obj is SaveSystemSettings && saveSystemSettingIcon != null)
                    EditorGUIUtility.SetIconForObject(obj, saveSystemSettingIcon);
            }
        }
    }
}