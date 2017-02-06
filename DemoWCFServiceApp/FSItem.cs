using System.Runtime.Serialization;

namespace DemoWCFServiceApp
{
    [DataContract]
    public class FSItem
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public long? size { get; set; }

        [DataMember]
        public bool isDirectory { get; set; }
    }

    //public class FItem : FSItem
    //{
    //    public FItem()
    //    {
    //        isDirectory = false;
    //    }
    //}

    //public class DItem : FSItem
    //{
    //    public DItem()
    //    {
    //        isDirectory = true;
    //    }
    //}

    //public class PItem : FSItem
    //{
    //    //public List<Int64> NestedItems { get; set; }
    //    public PItem()
    //    {
    //        isDirectory = false;
    //    }
    //}
}