using System.Collections.Generic;
using DemoWCFServiceApp.Repository;
using System;

namespace DemoWCFServiceApp
{
    public class FileService : IFileService
    {
        private IFileRepository FileRepository;

        public FileService()
        {
            FileRepository = new FilesRepository();
        }

        public IEnumerable<FSItem> GetAllFiles(string path)
        {
            if (path == null) path = AppDomain.CurrentDomain.BaseDirectory + "Fileserver";
            var allFile = FileRepository.GetAllFiles(path);
            return allFile;
        }

        public void AddFile(FSItem File)
        {
            FileRepository.AddFile(File);
        }

        public void DeleteFile(string path)
        {
            FileRepository.DeleteFile(path);
        }

        public void EditFile(string name, string newname)
        {
            FileRepository.EditFile(name, newname);
        }
    }
}
