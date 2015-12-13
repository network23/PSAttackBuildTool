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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Strings.psaWarning);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Strings.psaStartMsg);
            Console.ReadLine();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(Strings.psaLogo);
            Console.WriteLine("Version {0}\n", Strings.version);
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
                string encOutfile = punch.modules_dir + CryptoUtils.EncryptString(punch, module.Name) + ".ps1.enc";
                try
                {
                    PSAUtils.DownloadFile(module.URL, dest);
                    Console.WriteLine("[*] Encrypting: {0}", dest);
                    CryptoUtils.EncryptFile(punch, dest, encOutfile);
                }
                catch (Exception e)
                {
                    ConsoleColor origColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There was an error processing {0}. \nError message: \n\n{1}\n", module.Name, e.Message);
                    Console.ForegroundColor = origColor;
                }
            }
            Console.WriteLine("Generating PSPunch.csproj at {0}", punch.csproj_file);
            PSAUtils.BuildCsproj(modules, punch);
            Console.WriteLine("[*] Building PSPunch!");
            Console.ForegroundColor = ConsoleColor.Gray;
            int exitCode = PSAUtils.BuildPunch(punch);
            if (exitCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Strings.psaEndSuccess, Strings.punchBuildDir);
                Console.ReadLine();
                Process.Start(Strings.punchBuildDir);
            }
            else if (exitCode == 999)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Strings.psaEndNoMSBuild, System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
                Console.ReadLine();
                Environment.Exit(exitCode);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Strings.psaEndFailure);
                Console.ReadLine();
                Environment.Exit(exitCode);

            }

        }
    }
}
