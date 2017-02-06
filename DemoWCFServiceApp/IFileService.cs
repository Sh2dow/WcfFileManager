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

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "GET", UriTemplate = "DeleteFile?path={path}")]
        void DeleteFile(string path);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST", UriTemplate = "EditFile?name={name}&newname={newname}")]
        void EditFile(string name, string newname);
    }
}
