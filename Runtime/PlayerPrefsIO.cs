using UnityEngine;

namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// IO for saving via PlayerPrefs
    /// </summary>
    public sealed class PlayerPrefsIO : ISaveSystemIO
    {
        public string BasePath => "SaveFile";
        
        public bool Exists(string path) => PlayerPrefs.HasKey(path);
        public void Delete(string path) => PlayerPrefs.DeleteKey(path);
        public void Copy(string sourcePath, string destPath, bool overwrite)
        {
            if (Exists(sourcePath))
            {
                var data = ReadAllText(sourcePath);
                if (overwrite || !Exists(destPath))
                {
                    WriteAllText(destPath, data);
                }
            }
        }

        public string ReadAllText(string path) => PlayerPrefs.GetString(path, "");
        public void WriteAllText(string path, string contents)
        {
            PlayerPrefs.SetString(path, contents);
            PlayerPrefs.Save();
        }
    }
}