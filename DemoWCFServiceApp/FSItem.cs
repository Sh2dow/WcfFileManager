using System.Runtime.Serialization;

namespace DemoWCFServiceApp
{
    [DataContract]
    public class FSItem
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string path { get; set; }

        [DataMember]
        public bool isDirectory { get; set; }
    }

    //[DataContract]
    //public class FItem : FSItem
    //{
    //    public FItem()
    //    {
    //        isDirectory = false;
    //    }
    //}

    //[DataContract]
    //public class DItem : FSItem
    //{
    //    public DItem()
    //    {
    //        isDirectory = true;
    //    }
    //}

    //[DataContract]
    //public class PItem : FSItem
    //{
    //    //public List<Int64> NestedItems { get; set; }
    //    public PItem()
    //    {
    //        isDirectory = false;
    //    }
    //}
}