using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using PSAttackBuildTool.PSAttack;

namespace PSAttackBuildTool.Utils
{
    class CryptoUtils
    {
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

        public static string GenerateKey(Attack punch)
        {
            string keyPath = Path.Combine(punch.modules_dir, "key.txt");
            if (!(File.Exists(keyPath)))
            {
                string key = RandomString(64);
                File.WriteAllText(keyPath, key, Encoding.Unicode);
            }
            return File.ReadAllText(keyPath, Encoding.Unicode);
        }
        public static string HashString(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string EncryptString(Attack punch, string text)
        {
            string key = GenerateKey(punch);
            byte[] keyBytes;
            keyBytes = Encoding.Unicode.GetBytes(key);

            Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);

            RijndaelManaged rijndaelCSP = new RijndaelManaged();
            rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
            rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

            ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = encryptor.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer).Replace("/","_");
        }

        public static void EncryptFile(Attack punch, string inputFile, string outputFile)
        {
            string key = GenerateKey(punch);
            byte[] keyBytes;
            keyBytes = Encoding.Unicode.GetBytes(key);

            Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);

            RijndaelManaged rijndaelCSP = new RijndaelManaged();
            rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
            rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);

            ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();

            FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

            byte[] inputFileData = new byte[(int)inputFileStream.Length];
            inputFileStream.Read(inputFileData, 0, (int)inputFileStream.Length);

            FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

            CryptoStream encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write);
            encryptStream.Write(inputFileData, 0, (int)inputFileStream.Length);
            encryptStream.FlushFinalBlock();

            rijndaelCSP.Clear();
            encryptStream.Close();
            inputFileStream.Close();
            outputFileStream.Close();
        }
    }
}
