using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using FileService.WcfHost.Contracts;
using FileService.WcfHost.Helpers;

namespace FileService.WcfHost
{
    public class GeneralFileService : IFileService
    {
        private readonly string _fileResourcesPath;

        public GeneralFileService()
        {
            _fileResourcesPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "FileResources\\");
        }

        /// <summary>
        /// Download multiple files.
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public Stream Download(IEnumerable<string> fileIds)
        {
            if (fileIds == null)
                return null;

            var selectedFiles = GetSelectedFilesByFileIds(fileIds);
            return selectedFiles.Length > 0 ? ZipHelper.ZipToMemoryStream(selectedFiles) : null;
        }

        /// <summary>
        /// Upload one file. But you can call it many times when you want to upload multiple files.
        /// </summary>
        /// <param name="file"></param>
        public void Upload(DocInfo file)
        {
            if (file == null)
                return;
            var tempName = string.IsNullOrEmpty(file.FileName) ? "temp" : file.FileName;
            var writer = File.Create(_fileResourcesPath + Guid.NewGuid() + "@" + tempName);

            var buffer = new byte[4096];
            int byteRead;
            while ((byteRead = file.FileContent.Read(buffer, 0, buffer.Length)) != 0)
            {
                writer.Write(buffer, 0, byteRead);
            }

            file.FileContent.Close();
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Filename Format:[Guid]@[FileName].[ext]
        /// </summary>
        /// <returns></returns>
        public DocInfo[] GetFileInfos()
        {
            var infos = new List<DocInfo>(20);

            var files = Directory.GetFiles(_fileResourcesPath);
            infos.AddRange(from path in files
                           select Path.GetFileName(path)
                           into fullName where fullName != null select fullName.Split('@')
                           into parts where parts.Length == 2 select new DocInfo {Id = parts[0], FileName = parts[1]});

            return infos.ToArray();
        }

        /// <summary>
        /// Delete multiple files the user selected.
        /// </summary>
        /// <param name="fileIds"></param>
        public void DeleteFiles(IEnumerable<string> fileIds)
        {
            var selectedFiles = GetSelectedFilesByFileIds(fileIds);

            if (selectedFiles.Length == 0) return;

            foreach (var file in selectedFiles)
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Get the selected file paths according to fileIds.
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        private string[] GetSelectedFilesByFileIds(IEnumerable<string> fileIds)
        {
            if (fileIds == null)
                throw new ArgumentNullException("fileIds");

            var files = Directory.GetFiles(_fileResourcesPath);
            return fileIds.Select(fileId => files.FirstOrDefault(name => name.IndexOf(fileId, StringComparison.Ordinal) > -1))
                .Where(file => !string.IsNullOrEmpty(file)).ToArray();
        }
    }
}
