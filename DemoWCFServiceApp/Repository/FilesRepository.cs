using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DemoWCFServiceApp.Repository
{
    public class FilesRepository : IFileRepository
    {
        private List<FSItem> fsitems;
        private string localPath { get; set; }
        //private string filePath = AppDomain.CurrentDomain.BaseDirectory;

        public FilesRepository()
        {
        }

        IEnumerable<FSItem> IFileRepository.GetAllFiles(string path)
        {
            this.localPath = path == "secret" ? "secret" : HttpUtility.UrlDecode(path);
            //localPath = path == "secret" ? "secret" : path;
            var json = GetAllFiles(localPath).Result;
            return json;
        }

        public async Task<IEnumerable<FSItem>> GetAllFiles(string path)
        {
            var fsFolder = new DirectoryInfo(this.localPath);
            //path = HttpUtility.UrlPathEncode(fsFolder.FullName);
            fsitems = new List<FSItem>();
            if (localPath != "secret" && new DriveInfo(Directory.GetDirectoryRoot(localPath)).IsReady)
            {
                string fipath = "";
                if (Directory.GetDirectoryRoot(localPath) == localPath)
                    fipath = "secret";
                else
                    fipath = HttpUtility.UrlPathEncode(fsFolder.Parent.FullName);

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var files = fsFolder.EnumerateFiles()
                                                .Select(fi => new FSItem
                                                {
                                                    FileName = fi.Name,
                                                    path = path + @"\" + fi.Name,
                                                    isDirectory = false
                                                })
                                                .ToList();

                        var folders = fsFolder.EnumerateDirectories()
                                                    .Select(di => new FSItem
                                                    {
                                                        FileName = di.Name,
                                                        path = path + @"\" + di.Name,
                                                        isDirectory = true
                                                    })
                                                    .ToList();
                        fsitems.Add(new FSItem
                        {
                            path = fipath,
                            FileName = localPath,
                            isDirectory = true
                        });
                        fsitems.AddRange(folders);
                        fsitems.AddRange(files);
                    }
                    catch
                    {
                        fsitems.Add(new FSItem
                        {
                            path = fipath,
                            FileName = localPath,
                            isDirectory = true
                        });
                    }
                });

            }
            else
            {
                await Task.Factory.StartNew(() =>
                {
                    var drives = Environment.GetLogicalDrives()
                                                .Select(di => new FSItem
                                                {
                                                    FileName = di.FirstOrDefault().ToString() + @":\",
                                                    path = di.FirstOrDefault().ToString() + @":\",
                                                    isDirectory = true
                                                })
                                                .ToList();
                    fsitems.Add(new FSItem
                    {
                        path = "secret",
                        FileName = "My Computer",
                    });
                    fsitems.AddRange(drives);
                });
                return fsitems;
            }
            return fsitems;
        }

        public bool FileExists(string fileName)
        {
            var file = Directory.GetFiles(this.localPath, fileName)
                                .FirstOrDefault();
            return file != null;
        }
        
        public void AddFile(FSItem File)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(int id)
        {
            throw new NotImplementedException();
        }

        public void EditFile(FSItem File)
        {
            throw new NotImplementedException();
        }

        public int GetFilesCount()
        {
            throw new NotImplementedException();
        }
    }
}