using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace DemoWCFServiceApp
{
    [ServiceContract]
    public interface IFileService
    {
        // WebGet attribute is used to make GET request in WCF REST service
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "GET", UriTemplate = "GetAllFiles?path={path}")]
        //[WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetAllFiles?path={path}")]
        //[WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST", UriTemplate = "GetAllFiles/{path}")]
        IEnumerable<FSItem> GetAllFiles(string path);

        // WebInvoke attribute is used to make POST, DELETE and PUT request in WCF REST service
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST", UriTemplate = "AddFile")]
        void AddFile(FSItem File);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "DELETE", UriTemplate = "DeleteFile")]
        void DeleteFile(int id);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "PUT", UriTemplate = "EditFile")]
        void EditFile(FSItem File);
    }
}
