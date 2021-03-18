using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DesignService
{
    class AesHelp
    {
        /// <summary>
        /// 加密
        /// </summary>
        public static byte[] EncryptString(string str, string cryptPath, byte[] key, byte[] iv)
        {
            FileStream fsCrypt = new FileStream(cryptPath, FileMode.Create, FileAccess.Write);

            using (Aes aesAlg = Aes.Create())
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, iv);
                CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(str);
                }
                fsCrypt.Close();
                cs.Close();
            }
            fsCrypt = new FileStream(cryptPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fsCrypt);
            byte[] encrypted = br.ReadBytes(1024);
            return encrypted;
        }
        /// <summary>
        /// 解密
        /// </summary>
        public static string DescyrptString(string cryptPath, string plainPath, byte[] key, byte[] iv)
        {
            string str = null;
            FileStream fsCrypt = new FileStream(cryptPath, FileMode.Open, FileAccess.Read);
            using (Aes aesAlg = Aes.Create())
            {
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);
                CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read);
                FileStream fsPlaint = new FileStream(plainPath, FileMode.Create, FileAccess.Write);
                using (StreamReader sr = new StreamReader(cs))
                {
                    str = sr.ReadToEnd();
                }
                using (StreamWriter sw = new StreamWriter(fsPlaint))
                {
                    sw.Write(str);
                }
                fsCrypt.Close();
                fsPlaint.Close();
                cs.Close();
            }
            return str;
        }
        /// <summary>
        /// 根据提供的密码生成Key和IV
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static void GenKeyIV(string password, out byte[] key, out byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                key = new byte[aes.Key.Length];
                iv = new byte[aes.IV.Length];
                byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
                for (int i = 0; i < pwdBytes.Length; i++)
                {
                    key[i] = pwdBytes[i];
                    iv[i] = pwdBytes[i];
                }
            }
        }
        /// <summary>
        /// 随机生成Key和IV
        /// </summary>
        public static void GenKeyIV(out byte[] key, out byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                key = aes.Key;
                iv = aes.IV;
            }
        }
    }
}
