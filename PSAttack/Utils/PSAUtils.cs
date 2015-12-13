using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using PSAttack.Modules;
using PSAttack.PSPunch;

namespace PSAttack.Utils
{
    class PSAUtils
    {
        public static List<Module> GetModuleList(string JSON)
        {
            List<Module> moduleList = JsonConvert.DeserializeObject<List<Module>>(JSON);
            return moduleList;
        }

        public static void BuildCsproj(List<Module> modules, Punch punch)
        {
            punch.ClearCsproj();
            List<string> files = new List<string>();
            foreach (Module module in modules)
            {
                files.Add(CryptoUtils.EncryptString(punch, module.Name));
            }
            PSPunchCSProj csproj = new PSPunchCSProj();
            csproj.Session = new Dictionary<string, object>();
            csproj.Session.Add("files", files);
            csproj.Initialize();

            var generatedCode = csproj.TransformText();
            Console.WriteLine("Writing PSPunch.csproj to {0}", punch.csproj_file);
            File.WriteAllText(punch.csproj_file, generatedCode);
        }

        public static string DownloadFile(string url, string dest)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(url, dest);
            return dest;
        }

        public static string UnzipFile(string zipPath)
        {
            if (Directory.Exists(Strings.punchUnzipDir))
            {
                Directory.Delete(Strings.punchUnzipDir, true);
            }
            Directory.CreateDirectory(Strings.punchUnzipDir);
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                archive.ExtractToDirectory(Strings.punchUnzipDir);
                return Path.Combine(Strings.punchUnzipDir, archive.Entries[0].FullName);
            }
        }

        public static string GetPSAttackDir()
        {
            string PSAttackDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PSAttack");
            if (!(Directory.Exists(PSAttackDir)))
            {
                Directory.CreateDirectory(PSAttackDir);
            }
            return PSAttackDir+"\\";
        }

       public static Punch GetPSPunch(Uri URL)
        {
            WebClient wc = new System.Net.WebClient();
            // This took a while to figure out: https://developer.github.com/v3/#user-agent-required
            wc.Headers.Add("user-agent", Strings.githubUserAgent);
            string JSON = wc.DownloadString(URL);
            List<Punch> punchList = JsonConvert.DeserializeObject<List<Punch>>(JSON);
            return punchList[0];
        }

        public static int BuildPunch(Punch punch)
        {
            DateTime now = DateTime.Now;
            string buildDate = String.Format("{0:MMMM dd yyyy} at {0:hh:mm:ss tt}", now);
            using (StreamWriter buildDateFile = new StreamWriter(Path.Combine(punch.resources_dir, "attackDate.txt")))
            {
                buildDateFile.Write(buildDate);
            }
            string dotNetDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            string msbuildPath = Path.Combine(dotNetDir, "msbuild.exe");
            if (File.Exists(msbuildPath))
            {
                Process msbuild = new Process();
                msbuild.StartInfo.FileName = msbuildPath;
                msbuild.StartInfo.Arguments = punch.build_args;
                msbuild.StartInfo.UseShellExecute = false;
                msbuild.StartInfo.RedirectStandardOutput = true;
                msbuild.StartInfo.RedirectStandardError = true;

                Console.WriteLine("Running build with this command: {0} {1}", msbuild.StartInfo.FileName, msbuild.StartInfo.Arguments);

                msbuild.Start();
                string output = msbuild.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                string err = msbuild.StandardError.ReadToEnd();
                Console.WriteLine(err);
                msbuild.WaitForExit();
                int exitCode = msbuild.ExitCode;
                msbuild.Close();
                return exitCode;
            }
            return 999;
        }
    }
}
