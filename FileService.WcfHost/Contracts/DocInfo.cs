using System.IO;
using System.ServiceModel;

namespace FileService.WcfHost.Contracts
{
    [MessageContract]
    public class DocInfo
    {
        [MessageHeader]
        public string Id { get; set; }

        [MessageHeader]
        public string FileName { get; set; }

        [MessageHeader]
        public double FileSize { get; set; }

        [MessageBodyMember]
        public Stream FileContent { get; set; }
    }
}