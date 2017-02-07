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

        public FilesRepository()
        {
        }

        IEnumerable<FSItem> IFileRepository.GetAllFiles(string path)
        {
            localPath = path == "secret" ? "secret" : HttpUtility.UrlDecode(path);
            var json = GetAllFiles(localPath).Result;
            return json;
        }

        public async Task<IEnumerable<FSItem>> GetAllFiles(string path)
        {
            var fsFolder = new DirectoryInfo(this.localPath);
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
                                                    Attribute = fi.Length.ToString(),
                                                    isDirectory = false
                                                })
                                                .ToList();

                        var folders = fsFolder.EnumerateDirectories()
                                                    .Select(di => new FSItem
                                                    {
                                                        FileName = di.Name,
                                                        isDirectory = true
                                                    })
                                                    .ToList();
                        fsitems.Add(new FSItem
                        {
                            FileName = localPath + @"\",
                            Attribute = fipath,
                            isDirectory = true
                        });
                        fsitems.AddRange(folders);
                        fsitems.AddRange(files);
                    }
                    catch
                    {
                        fsitems.Add(new FSItem
                        {
                            FileName = localPath,
                            Attribute = fipath,
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
                                                    isDirectory = true
                                                })
                                                .ToList();
                    fsitems.Add(new FSItem
                    {
                        //FileName = "My Computer",
                        FileName = "",
                        Attribute = "secret"
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

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void EditFile(string path, string name)
        {
            var location = new FileInfo(path).Directory.FullName;
            File.Move(path, location + @"\" + name);
        }
    }
}