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
            //FileRepository = new FileXmlRepository();
            FileRepository = new FilesRepository();
        }

        public IEnumerable<FSItem> GetAllFiles()
        {
            var allFile = FileRepository.GetAllFiles();
            return allFile;
        }

        public void AddFile(FSItem File)
        {
            FileRepository.AddFile(File);
        }

        public void DeleteFile(int id)
        {
            FileRepository.DeleteFile(id);
        }

        public void EditFile(FSItem File)
        {
            FileRepository.EditFile(File);
        }
    }
}
