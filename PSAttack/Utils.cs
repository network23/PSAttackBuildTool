using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using Newtonsoft.Json;

namespace PSAttack
{
    class Utils
    {
        public static Dictionary<string, string> GetModuleDict(string JSON)
        {
            Dictionary<string, string> jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);
            return jsonDict;
        }

        public static string DownloadFile(string url, string dest)
        {
            string DownloadPath = Path.Combine(GetPSAttackDir(), dest);
            WebClient wc = new WebClient();
            wc.DownloadFile(url, DownloadPath);
            return DownloadPath;
        }

        public static string GetPSAttackDir()
        {
            string PSAttackDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PSAttack");
            if (!(Directory.Exists(PSAttackDir)))
            {
                Directory.CreateDirectory(PSAttackDir);
            }
            return PSAttackDir;
        }

        public static string RandomString(int size)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString(); ;
        }

        public static string GenerateKey()
        {
            string keyPath = Path.Combine(GetPSAttackDir(), "key.txt");
            if (!(File.Exists(keyPath)))
            {
                string key = RandomString(64);
                File.WriteAllText(keyPath, key, Encoding.Unicode);
            }
            return File.ReadAllText(keyPath, Encoding.Unicode);
        }
    }
}
