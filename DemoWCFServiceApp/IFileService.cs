using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DemoWCFServiceApp
{
    [ServiceContract]
    public interface IFileService
    {
        // WebGet attribute is used to make GET request in WCF REST service
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetAllFiles")]
        IEnumerable<FSItem> GetAllFiles();

        // WebInvoke attribute is used to make POST, DELETE and PUT request in WCF REST service
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST", UriTemplate = "AddFile")]
        void AddFile(FSItem File);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "DELETE", UriTemplate = "DeleteFile")]
        void DeleteFile(int id);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "PUT", UriTemplate = "EditFile")]
        void EditFile(FSItem File);
    }
}
