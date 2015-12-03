using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PSAttack.Modules
{
    public class ModuleJSON
    {
        [JsonProperty("module")]
        public Module Module { get; set; }
    }

    public class Module
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }
    }
}
