using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PSAttack.Modules
{
    public class ModuleJSON
    {
        public Module Module { get; set; }
    }

    [DataContract]
    public class Module
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string url { get; set; }
    }
}
