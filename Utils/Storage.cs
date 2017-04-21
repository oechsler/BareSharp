using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace BareKit
{
    public static class Storage
    {
        static IsolatedStorageFile container;

        /// <summary>
        /// Initalizes the Storage filesystem.
        /// </summary>
        public static void Initalize()
        {
#if !WINDOWS_UAP
            container = IsolatedStorageFile.GetUserStoreForAssembly();
#else
            container = IsolatedStorageFile.GetUserStoreForApplication();
#endif
        }

        /// <summary>
        /// Reads a file from the filesystem.
        /// </summary>
        /// <param name="path">The relative path to the file.</param>
        public static Stream Read(string path)
        {
            return container.FileExists(path) ? container.OpenFile(path, FileMode.Open) : null;
        }

        /// <summary>
        /// Writes a file to the filesystem.
        /// </summary>
        /// <param name="path">The relative path to the file.</param>
        /// <param name="append">Whether the input is appended to the file.</param>
        /// <returns></returns>
        public static Stream Write(string path, bool append = false)
        {
            return append ? container.OpenFile(path, FileMode.Append) : container.CreateFile(path);
        }

        /// <summary>
        /// Deletes a file from the filesystem.
        /// </summary>
        /// <param name="path">The relative path to the file.</param>
        public static void Delete(string path)
        {
            if (container.FileExists(path))
                container.DeleteFile(path);
            else if (container.DirectoryExists(path))
                container.DeleteDirectory(path);
        }

        /// <summary>
        /// Whether a file exists in the filesystem.
        /// </summary>
        /// <param name="path">The relative path to the file.</param>
        public static bool Exists(string path)
        {
            return container.FileExists(path) || container.DirectoryExists(path);
        }

        /// <summary>
        /// Reads a file marked as embedded resource from the executing assembly.
        /// </summary>
        /// <param name="path">The realtive path to the embedded resource.</param>
        public static Stream EmbeddedResource(string path)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(path.Replace("/", "."));
        }
    }
}
