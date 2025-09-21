using System.IO;
using UnityEngine;

namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// IO for saving via fle system
    /// </summary>
    public sealed class FileSystemIO : ISaveSystemIO
    {
        public string BasePath => Application.persistentDataPath;
        
        public bool Exists(string path) => File.Exists(path);
        public void Delete(string path) => File.Delete(path);
        public void Copy(string sourcePath, string destPath, bool overwrite) =>
            File.Copy(sourcePath, destPath, overwrite);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
    }
}