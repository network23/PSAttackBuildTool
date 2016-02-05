using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAttackBuildTool.Modules;
using PSAttackBuildTool.Utils;
using PSAttackBuildTool.PSAttack;
using System.Diagnostics;

namespace PSAttackBuildTool
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
            Random random = new Random();
            int psaLogoInt = random.Next(Strings.psabtLogos.Count);
            Console.WriteLine(Strings.psabtLogos[psaLogoInt]);
            Console.WriteLine("Version {0}\n", Strings.version);
            Console.ForegroundColor = ConsoleColor.White;

            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();
            MemoryStream memReader = new MemoryStream(Encoding.UTF8.GetBytes(modulesJson));

            Console.WriteLine("[*] Getting modules from local json.");
            List<Module> modules = PSABTUtils.GetModuleList(memReader);
            string workingDir = PSABTUtils.GetPSAttackBuildToolDir();

            Console.WriteLine("[*] Looking for latest release of PS>Attack");
            Attack punch = PSABTUtils.GetPSPunch(new Uri(Strings.attackURL));

            Console.WriteLine("[*] Got Punch Version: {0}", punch.tag_name);
            Console.WriteLine("[*] Downloading: {0}", punch.zipball_url);
            punch.DownloadZip();
            Console.WriteLine("[*] Unzipping to: {0}", Strings.attackUnzipDir);
            punch.unzipped_dir = PSABTUtils.UnzipFile(Strings.attackZipPath);

            Console.WriteLine("[*] Clearing modules at: {0}", punch.modules_dir);
            punch.ClearModules();
            
            if (!(Directory.Exists(Strings.moduleSrcDir)))
            {
                Directory.CreateDirectory(Strings.moduleSrcDir);
            }

            foreach (Module module in modules)
            {
                string dest = Path.Combine(Strings.moduleSrcDir, (module.name + ".ps1"));
                string encOutfile = punch.modules_dir + CryptoUtils.EncryptString(punch, module.name) + ".ps1.enc";
                try
                {
                    PSABTUtils.DownloadFile(module.url, dest);
                    Console.WriteLine("[*] Encrypting: {0}", dest);
                    CryptoUtils.EncryptFile(punch, dest, encOutfile);
                }
                catch (Exception e)
                {
                    ConsoleColor origColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There was an error processing {0}. \nError message: \n\n{1}\n", module.name, e.Message);
                    Console.ForegroundColor = origColor;
                }
            }
            Console.WriteLine("Generating PSAttack.csproj at {0}", punch.csproj_file);
            PSABTUtils.BuildCsproj(modules, punch);
            Console.WriteLine("[*] Building PSAttack!");
            Console.ForegroundColor = ConsoleColor.Gray;
            int exitCode = PSABTUtils.BuildPunch(punch);
            if (exitCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Strings.psaEndSuccess, Strings.attackBuildDir);
                Console.ReadLine();
                Process.Start(Strings.attackBuildDir);
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
