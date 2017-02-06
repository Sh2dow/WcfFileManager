using WebApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    [Authorize]
    public class MVCHomeController : Controller
    {
        readonly string FileServiceUri = "http://localhost:1786/FileService.svc/";

        public ActionResult Index()
        {
            var FileList = new List<FSItem>();
            using (var webClient = new WebClient())
            {
                var dwml = webClient.DownloadString(FileServiceUri + "GetAllFiles?path=" + HttpRuntime.AppDomainAppPath);
                FileList.AddRange(JsonConvert.DeserializeObjectAsync<List<FSItem>>(dwml).Result);
            }
            return View(FileList);
        }

        [HttpGet]
        public ActionResult GetFile()
        {
            return PartialView("CreateFile");
        }

        [HttpPost]
        public ActionResult CreateFile(FSItem File)
        {
            using (WebClient wc = new WebClient())
            {

                var ms = new MemoryStream();
                var serializerToUplaod = new DataContractJsonSerializer(typeof(FSItem));
                serializerToUplaod.WriteObject(ms, File);

                wc.Headers["Content-type"] = "application/json";
                wc.UploadData(FileServiceUri + "AddFile", "POST", ms.ToArray());
            }

            int pageToShow;
            int totalFiles;
            var FileList = new List<FSItem>();

            using (var webClient = new WebClient())
            {
                string FileCount;
                FileCount = webClient.DownloadString(FileServiceUri + "GetFilesCount");
                totalFiles = Convert.ToInt32(FileCount);
            }

            if (totalFiles % 5 != 0)
                pageToShow = (totalFiles / 5) + 1;
            else pageToShow = totalFiles / 5;

            return Redirect(HttpRuntime.AppDomainAppVirtualPath + "?page=" + pageToShow);
        }

        public void DeleteFile(int id)
        {
            using (var wc = new WebClient())
            {

                var ms = new MemoryStream();
                var serializerToUplaod = new DataContractJsonSerializer(typeof(FSItem));
                serializerToUplaod.WriteObject(ms, id);

                wc.Headers["Content-type"] = "application/json";
                wc.UploadData(FileServiceUri + "DeleteFile", "DELETE", ms.ToArray());
            }
        }

        public void EditFile(FSItem File)
        {
            using (WebClient wc = new WebClient())
            {

                var ms = new MemoryStream();
                var serializerToUplaod = new DataContractJsonSerializer(typeof(FSItem));
                serializerToUplaod.WriteObject(ms, File);

                wc.Headers["Content-type"] = "application/json";
                wc.UploadData(FileServiceUri + "EditFile", "PUT", ms.ToArray());
            }
        }
    }
}