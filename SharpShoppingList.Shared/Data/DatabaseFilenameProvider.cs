using System;
using System.IO;

namespace SharpShoppingList.Data
{
    public class DatabaseFilenameProvider
    {
        private readonly string _filename;

        public DatabaseFilenameProvider(string filename)
        {
            _filename = filename;
        }

        public string DatabaseFilePath
        {
            get
            {
#if NETFX_CORE
                var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
#else

#if SILVERLIGHT
                // Windows Phone expects a local path, not absolute
                var path = sqliteFilename;
#else

#if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
#endif
                var path = Path.Combine(libraryPath, _filename);
#endif

#endif
                return path;
            }
        }
    }
}