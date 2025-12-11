
namespace WolverineSoft.SaveSystem
{
    /// <summary>
    /// Abstraction for Save System file operations.
    /// Use SaveSystemIO to access platform-specific implementations.
    /// </summary>
    public static class SaveSystemIO
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            private static ISaveSystemIO _impl = new PlayerPrefsIO();
        #else
            private static ISaveSystemIO _impl = new FileSystemIO();
        #endif
        
        public static string BasePath => _impl.BasePath;

        public static bool Exists(string path) => _impl.Exists(path);
        public static void Delete(string path) => _impl.Delete(path);
        public static void Copy(string sourcePath, string destPath, bool overwrite) =>
            _impl.Copy(sourcePath, destPath, overwrite);
        public static string ReadAllText(string path) => _impl.ReadAllText(path);
        public static void WriteAllText(string path, string contents) => _impl.WriteAllText(path, contents);
    }

    public interface ISaveSystemIO
    {
        string BasePath { get; }
        
        bool Exists(string path);
        void Delete(string path);
        void Copy(string sourcePath, string destPath, bool overwrite);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }
}