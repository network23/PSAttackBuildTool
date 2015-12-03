using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAttack.Modules;
using PSAttack.Utils;


namespace PSAttack
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();
            List<Module> modules = PSAUtils.GetModuleList(modulesJson);
            string workingDir = PSAUtils.GetPSAttackDir();
            foreach (Module module in modules)
            {
                string dest = workingDir + module.Name + ".ps1";
                string encOutfile = dest + ".enc";
                Console.WriteLine(dest);
                PSAUtils.DownloadFile(module.URL, dest);
                CryptoUtils.EncryptFile(dest, encOutfile);
            }
            Console.ReadLine();
        }
    }
}
