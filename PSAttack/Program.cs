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
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(Strings.psaLogo);
            Console.ForegroundColor = ConsoleColor.White;
            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();
            Console.WriteLine("[*] Getting modules from local json.");
            List<Module> modules = PSAUtils.GetModuleList(modulesJson);
            string workingDir = PSAUtils.GetPSAttackDir();

            Console.WriteLine("[*] Looking for latest release of PS>Punch");
            Punch punch = PSAUtils.GetPSPunch(new Uri(Strings.punchURL));

            Console.WriteLine("[*] Got Punch Version: {0}", punch.tag_name);
            Console.WriteLine("[*] Downloading: {0}", punch.zipball_url);
            punch.DownloadZip();
            Console.WriteLine("[*] Unzipping to: {0}", Strings.punchUnzipDir);
            punch.unzipped_dir = PSAUtils.UnzipFile(Strings.punchZipPath);

            Console.WriteLine("[*] Clearing modules at: {0}", punch.modules_dir);
            punch.ClearModules();
            
            if (!(Directory.Exists(Strings.moduleSrcDir)))
            {
                Directory.CreateDirectory(Strings.moduleSrcDir);
            }

            foreach (Module module in modules)
            {
                string dest = Path.Combine(Strings.moduleSrcDir, (module.Name + ".ps1"));
                string encOutfile = punch.modules_dir + module.Name + ".ps1.enc";
                PSAUtils.DownloadFile(module.URL, dest);
                Console.WriteLine("[*] Encrypting: {0}", dest);
                CryptoUtils.EncryptFile(punch, dest, encOutfile);
            }
            Console.WriteLine("[*] Building PSPunch!");
            Console.ForegroundColor = ConsoleColor.Gray;
            PSAUtils.BuildPunch(punch);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"

    Build complete! Some warning messages are expected from the compiler 
    so don't be alarmed. If there are errors running your build of 
    PS>Punch, please submit an issue on github referencing the errors
    that came up during the build.

    Your build of PS>Punch is available at: 

    {0}

    You only need the PSPunch.exe file, the others are extra from the
    build process.

    Press return to open up the folder. Thanks for using PS>Attack!

", Strings.punchBuildDir);
            Console.ReadLine();
            Process.Start(Strings.punchBuildDir);
        }
    }
}
