using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace FileService.WcfHost.Helpers
{
    public class ZipHelper
    {
        public static Stream ZipToMemoryStream(IEnumerable<string> filePaths)
        {
            var zipFile = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(zipFile) { IsStreamOwner = false };
            zipOutputStream.SetLevel(6);
            foreach (var path in filePaths)
            {
                var entry = new ZipEntry(GetFileNameInZip(Path.GetFileName(path)))
                {
                    DateTime = DateTime.Now
                };
                zipOutputStream.PutNextEntry(entry);
                var source = File.OpenRead(path);
                StreamUtils.Copy(source, zipOutputStream, new byte[4096]);

                zipOutputStream.CloseEntry();
                source.Close();
            }
            zipOutputStream.Finish();
            zipOutputStream.Close();

            zipFile.Position = 0;
            zipFile.Flush();

            return zipFile;
        }

        /// <summary>
        /// Get file name in zip file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetFileNameInZip(string fileName)
        {
            var parts = fileName.Split('@');
            if (parts.Length == 2)
            {
                return parts[1];
            }
            return string.Empty;
        }
    }
}