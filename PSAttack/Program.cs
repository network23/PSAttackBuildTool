using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAttack.Modules;
using PSAttack.Utils;
using PSAttack.PSPunch;
using System.Diagnostics;

namespace PSAttack
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();

            Console.WriteLine("Getting modules from local json");
            List<Module> modules = PSAUtils.GetModuleList(modulesJson);
            string workingDir = PSAUtils.GetPSAttackDir();

            Console.WriteLine("Creating PSPUnch obj");
            Punch punch = PSAUtils.GetPSPunch(new Uri(Strings.punchURL));

            Console.WriteLine("Got Punch Version: {0}", punch.tag_name);

            Console.WriteLine("Download Zip");
            punch.DownloadZip();

            Console.WriteLine("Unzipping zip");
            punch.unzipped_dir = PSAUtils.UnzipFile(Strings.punchZipPath);

            Console.WriteLine("Extacted to: {0}", punch.unzipped_dir);

            Console.WriteLine("Clearing modules at: {0}", punch.modules_dir);
            punch.ClearModules();
            
            if (!(Directory.Exists(Strings.moduleSrcDir)))
            {
                Directory.CreateDirectory(Strings.moduleSrcDir);
            }

            foreach (Module module in modules)
            {
                string dest = Path.Combine(Strings.moduleSrcDir, (module.Name + ".ps1"));
                string encOutfile = punch.modules_dir + module.Name + ".ps1.enc";
                Console.WriteLine("Downloading: {0}", dest);
                PSAUtils.DownloadFile(module.URL, dest);
                Console.WriteLine("Encrypting: {0}", dest);
                CryptoUtils.EncryptFile(punch, dest, encOutfile);
            }
            Console.WriteLine("Building PSPunch!");
            PSAUtils.BuildPunch(punch);
            Process.Start(Strings.punchBuildDir);
            Console.ReadLine();
        }
    }
}
