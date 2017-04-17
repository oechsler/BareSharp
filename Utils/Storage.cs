using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace BareKit
{
    public static class Storage
    {
        static IsolatedStorageFile container;

        public static void Initalize()
        {
#if !WINDOWS_UAP
            container = IsolatedStorageFile.GetUserStoreForAssembly();
#else
            container = IsolatedStorageFile.GetUserStoreForApplication();
#endif
        }

        public static Stream Read(string path)
        {
            return container.FileExists(path) ? container.OpenFile(path, FileMode.Open) : null;
        }

        public static Stream Write(string path, bool append = false)
        {
            return append ? container.OpenFile(path, FileMode.Append) : container.CreateFile(path);
        }

        public static void Delete(string path)
        {
            if (container.FileExists(path))
                container.DeleteFile(path);
            else if (container.DirectoryExists(path))
                container.DeleteDirectory(path);
        }

        public static bool Exists(string path)
        {
            return container.FileExists(path) || container.DirectoryExists(path);
        }

        public static Stream EmbeddedResource(string path)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(path.Replace("/", "."));
        }
    }
}
