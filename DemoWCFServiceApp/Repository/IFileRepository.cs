using System.Collections.Generic;

namespace DemoWCFServiceApp.Repository
{
    interface IFileRepository
    {
        IEnumerable<FSItem> GetAllFiles(string path );
        void AddFile(FSItem File);
        void DeleteFile(string path);
        void EditFile(string path, string name);
    }
}
