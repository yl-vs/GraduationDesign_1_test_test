using DesignService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace DesignService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“CryptoService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 CryptoService.svc 或 CryptoService.svc.cs，然后开始调试。
    public class CryptoService : ICryptoService
    {
        #region 登录和注册功能
        MyDbEntities context = new MyDbEntities();
        private string filePath;
        public int Login(string username, string password)
        {
            var q = from t in context.UserTable
                    where t.UserName == username && t.UserPasswd == password
                    select t;
            return q.Count();
        }

        //添加用户
        public bool AddUser(string name, string password)
        {
            UserTable user = new UserTable();
            user.UserName = name;
            user.UserPasswd = password;
            user.PrivateKey = name + "_privateKey.xml";
            user.PublicKey = name + "_publicKey.xml";
            CreateKey(name);
            //showUsers();
            try
            {
                context.UserTable.Add(user);
                int i = context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        //创建RSA公私钥对
        private void CreateKey(string userName)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string priPath = @AppDomain.CurrentDomain.BaseDirectory +"/PrivateKey/" + userName + "_privateKey.xml";
            string pubPath = @AppDomain.CurrentDomain.BaseDirectory + "/PublicKey/" + userName + "_publicKey.xml";

            FileStream fs1 = new FileStream(priPath, FileMode.Create, FileAccess.Write);
            FileStream fs2 = new FileStream(pubPath, FileMode.Create, FileAccess.Write);
            fs1.Close(); fs2.Close();
            //私钥
            using (StreamWriter sw = new StreamWriter(priPath))
            {
                sw.WriteLine(rsa.ToXmlString(true));
            }

            //公钥
            using (StreamWriter sw = new StreamWriter(pubPath))
            {
                sw.WriteLine(rsa.ToXmlString(false));
            }
        }
        #endregion

        #region 上传功能
        //获取用户ID
        private int getID(string name)
        {
            var q = from t in context.UserTable
                    where t.UserName == name
                    select t.UserId;
            return q.First();
        }

        public List<string> showUsers()
        {
            var context = new MyDbEntities();
            //step1 1
            if (context != null)
            {
                context.Dispose();
                context = new MyDbEntities();
            }
            var q = from t in context.UserTable
                    select t.UserName;
            //step 3
            return q.ToList();
        }
        private bool saveFile(FileTable file)
        {
            try
            {
                context.FileTable.Add(file);
                int i = context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string choseFile(string FilePath)
        {
            FileTable file = new FileTable();

            //打开文件选择器，并选择文件
            //var dialog = new OpenFileDialog();
            //if (dialog.ShowDialog(this) == false) return;
            //记录文件所处路径，并显示文件名
            filePath = FilePath;
            file.FilePath = FilePath;
            string[] fileName = FilePath.Split('\\');
            string FileName = fileName[fileName.Length - 1];
            file.FileName = FileName;
            return FileName;
        }
        public void createFile(string strContent, string userName, string downloadName)
        {
            byte[] key, iv;
            string[] fileName = filePath.Split('\\');
            string FileName = fileName[fileName.Length - 1];
            //加密文件内容
            AesHelp.GenKeyIV(out key, out iv);
            AesHelp.EncryptString(strContent, AppDomain.CurrentDomain.BaseDirectory + "/files/" + userName + "_" + FileName, key, iv);
            FileTable file = new FileTable();
            file.UploadId = getID(userName);
            file.FileName = FileName;
            file.FilePath = AppDomain.CurrentDomain.BaseDirectory + "/files/" + userName + "_" + FileName;

            file.DownloadId = getID(downloadName);
            file.SessionKey = saveSessionKey(key, iv, downloadName, FileName);
            saveFile(file);
        }

        //加密并保存会话密钥
        private string saveSessionKey(byte[] key, byte[] iv, string name, string FileName)
        {
            string publicKey, keyPath, sessionKey;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //导入下载方RSA公钥
            string pubPath = @AppDomain.CurrentDomain.BaseDirectory + "/PublicKey/" + name + "_publicKey.xml";
            using (StreamReader sr = new StreamReader(pubPath))
            {
                publicKey = sr.ReadLine();
            }
            rsa.FromXmlString(publicKey);
            //使用下载方公钥加密会话密钥
            byte[] keySession = rsa.Encrypt(key, false);
            byte[] ivSession = rsa.Encrypt(iv, false);
            //存储加密后的会话密钥
            FileName = FileName.Split('.')[0];
            keyPath = @AppDomain.CurrentDomain.BaseDirectory + "/SessionKey/" + name + "_" + FileName + "_RSAkey.xml";
            sessionKey = Convert.ToBase64String(keySession) + "," + Convert.ToBase64String(ivSession);
            FileStream fs = new FileStream(keyPath, FileMode.Create, FileAccess.Write);
            fs.Close();
            //key,iv
            using (StreamWriter sw = new StreamWriter(keyPath))
            {
                sw.WriteLine(sessionKey);
            }
            return keyPath;
        }


        #endregion

        #region 下载功能
        //显示用户可以下载的文件
        public List<string> DisplayFiles(string userName)
        {
            var q = from t in context.FileTable
                    from m in context.UserTable
                    where m.UserName == userName && t.DownloadId == m.UserId
                    select t.FileName;
            return q.ToList();
        }
        public List<string> FindFile(string file, string userName)
        {
            var q = from t in context.FileTable
                    from m in context.UserTable
                    where t.FileName.Contains(file) && m.UserName == userName && t.DownloadId == m.UserId
                    select t.FileName;
            return q.ToList();
        }
        private void decrypt(string priPath, string sessionPath, string filePath, string downloadPath)
        {
            string privateKey, sessionKey;
            byte[] enkey, eniv;
            using (StreamReader sr = new StreamReader(sessionPath))
            {
                sessionKey = sr.ReadToEnd();
            }
            string[] s = sessionKey.Split(',');
            enkey = Convert.FromBase64String(s[0]);
            eniv = Convert.FromBase64String(s[1]);

            using (StreamReader sr = new StreamReader(priPath))
            {
                privateKey = sr.ReadLine();
            }
            //解密会话密钥
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            byte[] key = rsa.Decrypt(enkey, false);
            byte[] iv = rsa.Decrypt(eniv, false);
            //解密文件
            string[] fileName = filePath.Split('/');
            string downFile = downloadPath + "\\" + fileName[fileName.Length - 1];
            string deContent = AesHelp.DescyrptString(filePath, downFile, key, iv);
            using (StreamWriter sw = new StreamWriter(downFile))
            {
                sw.WriteLine(deContent);
            }
            //this.txt.Text = AesHelp.DescyrptString("../../files/" + userName + "_" + FileName, decrypt, iv);
            //string asd = Convert.ToBase64String(iv);//存储iv
            //this.txt.Text = Convert.ToBase64String(iv);
            //解密key,iv  byte[] ds = Convert.FromBase64String(asd);
        }

        public void DecryptFile(string filename, string username, string downloadPath)
        {
            string prikey;
            var q = from t in context.FileTable
                    from m in context.UserTable
                    where t.FileName == filename && m.UserName == username
                    && t.DownloadId == m.UserId
                    select new { t.FilePath, t.SessionKey, m.PrivateKey };
            foreach (var item in q)
            {
                prikey = @AppDomain.CurrentDomain.BaseDirectory + "/PrivateKey/" + item.PrivateKey;
                decrypt(prikey, item.SessionKey, item.FilePath, downloadPath);
            }
        }
        #endregion
    }
}
