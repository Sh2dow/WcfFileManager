using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace FileService.WcfHost.Contracts
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        Stream Download(IEnumerable<string> fileIds);

        [OperationContract]
        void Upload(DocInfo file);

        [OperationContract]
        DocInfo[] GetFileInfos();

        [OperationContract]
        void DeleteFiles(IEnumerable<string> fileIds);
    }
}
