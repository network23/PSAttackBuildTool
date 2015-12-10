using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

using PSAttack.Utils;

namespace PSAttack.PSPunch
{
    class Punch
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tag_name")]
        public string tag_name { get; set; }

        [JsonProperty("zipball_url")]
        public string zipball_url { get; set; }

        [JsonProperty("published_at")]
        public DateTime published_at { get; set; }

        public string unzipped_dir { get; set; }

        public string modules_dir {
            get
            {
                return Path.Combine(this.unzipped_dir, Strings.punchModulesDir);
            }
        }

        public string res_dir
        {
            get
            {
                return Path.Combine(this.unzipped_dir, Strings.punchResDir);
            }
        }
        public string build_args
        {
            get
            {
                string solutionPath = Path.Combine(this.unzipped_dir, "PSPunch.sln");
                return solutionPath + " /p:Configuration=Debug /p:OutputPath=" + Strings.punchBuildDir;
            }
        }

        public void DownloadZip()
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", Strings.githubUserAgent);
            wc.DownloadFile(this.zipball_url, Strings.punchZipPath);
        }

        public void ClearModules()
        {
            try
            {
                Directory.Delete(this.modules_dir, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not clear out modules dir at {0}.\n Error message {1}", this.modules_dir, e.Message);
            }
            if (!(Directory.Exists(this.modules_dir)))
            {
                Directory.CreateDirectory(this.modules_dir);
            }
        }
    }
}
