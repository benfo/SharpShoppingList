using SharpShoppingList.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpShoppingList.Data
{
    public class ListRepository : IListRepository
    {
        private readonly Database _database;

        public ListRepository()
        {
            _database = new Database(DatabaseFilePath);
        }

        private static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "SharpShoppingList.db3";
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
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
#endif
                var path = Path.Combine(libraryPath, sqliteFilename);
#endif

#endif
                return path;
            }
        }

        public IEnumerable<List> GetLists()
        {
            return _database.GetLists();
        }

        public int AddList(List list)
        {
            return _database.SaveItem(list);
        }

        public int DeleteList(int id)
        {
            return _database.DeleteItem(id);
        }
    }
}