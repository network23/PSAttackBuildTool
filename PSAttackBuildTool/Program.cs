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
            // PRINT START MESSAGE
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Strings.psaStartMsg);
            Console.ReadLine();
            Console.Clear();

            //PRINT LOGO
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Random random = new Random();
            int psaLogoInt = random.Next(Strings.psabtLogos.Count);
            Console.WriteLine(Strings.psabtLogos[psaLogoInt]);
            Console.WriteLine("Version {0}\n", Strings.version);
            Console.ForegroundColor = ConsoleColor.White;

            //READ JSON FILE
            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();
            MemoryStream memReader = new MemoryStream(Encoding.UTF8.GetBytes(modulesJson));
            Console.WriteLine("[*] Getting modules from local json.");
            List<Module> modules = PSABTUtils.GetModuleList(memReader);
            string workingDir = PSABTUtils.GetPSAttackBuildToolDir();

            //GET PS>ATTACK 
            Console.WriteLine("[*] Looking for latest release of PS>Attack");
            Attack attack = PSABTUtils.GetPSAttack(new Uri(Strings.attackURL));
            Console.WriteLine("[*] Got PS>Attack Version: {0}", attack.tag_name);
            Console.WriteLine("[*] Downloading: {0}", attack.zipball_url);
            attack.DownloadZip();
            Console.WriteLine("[*] Unzipping to: {0}", Strings.attackUnzipDir);
            attack.unzipped_dir = PSABTUtils.UnzipFile(Strings.attackZipPath);

            // CLEAR OUT BUNDLED MODULES
            Console.WriteLine("[*] Clearing modules at: {0}", attack.modules_dir);
            attack.ClearModules();
            
            if (!(Directory.Exists(Strings.moduleSrcDir)))
            {
                Directory.CreateDirectory(Strings.moduleSrcDir);
            }

            // MAKE NEW MODULES
            foreach (Module module in modules)
            {
                string dest = Path.Combine(Strings.moduleSrcDir, (module.name + ".ps1"));
                string encOutfile = attack.modules_dir + CryptoUtils.EncryptString(attack, module.name) + ".ps1.enc";
                try
                {
                    PSABTUtils.DownloadFile(module.url, dest);
                    Console.WriteLine("[*] Encrypting: {0}", dest);
                    CryptoUtils.EncryptFile(attack, dest, encOutfile);
                }
                catch (Exception e)
                {
                    ConsoleColor origColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There was an error processing {0}. \nError message: \n\n{1}\n", module.name, e.Message);
                    Console.ForegroundColor = origColor;
                }
            }

            // GENERATE CSPROJ FILE
            Console.WriteLine("Generating PSAttack.csproj at {0}", attack.csproj_file);
            PSABTUtils.BuildCsproj(modules, attack);

            // BUILD PS>ATTACK
            Console.WriteLine("[*] Building PS>Attack!");
            Console.ForegroundColor = ConsoleColor.Gray;
            int exitCode = PSABTUtils.BuiltPSAttack(attack);
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
