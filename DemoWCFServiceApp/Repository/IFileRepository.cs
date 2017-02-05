using System.Collections.Generic;

namespace DemoWCFServiceApp.Repository
{
    interface IFileRepository
    {
        IEnumerable<FSItem> GetAllFiles();
        void AddFile(FSItem File);
        void DeleteFile(int id);
        void EditFile(FSItem File);
    }
}
