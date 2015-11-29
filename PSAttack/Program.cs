using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PSAttack
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();
            Dictionary<string, string> modules = Utils.GetModuleDict(modulesJson);
            foreach (KeyValuePair<string,string> module in modules)
            {

            }
        }
    }
}
