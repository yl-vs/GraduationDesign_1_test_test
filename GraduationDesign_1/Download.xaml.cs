using System.Linq;
using System.Windows;
using GraduationDesign_1.Models;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Windows.Forms;

namespace GraduationDesign_1
{
    /// <summary>
    /// Download.xaml 的交互逻辑
    /// </summary>
    public partial class Download : Window
    {
        public string userName { get; set; }
        public MyDbEntities context = new MyDbEntities();
        public Download(string name)
        {
            userName = name;
            InitializeComponent();
            this.Filelist.Items.Clear();
            DisplayFiles();
        }
        private int getID(string name)
        {
            var q = from t in context.UserTable
                    where t.UserName == name
                    select t.UserId;
            return q.First();
        }
        //显示用户可以下载的文件
        private void DisplayFiles()
        {

            var q = from t in context.FileTable
                    from m in context.UserTable
                    where m.UserName == userName && t.DownloadId == m.UserId
                    select t.FileName;
            this.Filelist.ItemsSource = q.ToList();

        }
        ////将用户选中的选项删除同时需要删除数据库中的相应数据段
        //private void D_clear_Click(object sender, RoutedEventArgs e)
        //{
        //    for (int i = Filelist.SelectedItems.Count - 1; i >= 0; i--)
        //    {
        //        string s = (string)Filelist.SelectedItems[i];
        //        //删除filelist中的选定项
        //        Filelist.Items.Remove(Filelist.SelectedItems[i]);
        //        //删除数据库中的数据项
        //        var q = from t in context.FileTable
        //                where t.FilePath == s
        //                select t;
        //        foreach (var v in q)
        //        {
        //            context.FileTable.Remove(v);
        //        }
        //    }
        //}
        //搜索用户想要下载的文件
        private void D_retrieve_Click(object sender, RoutedEventArgs e)
        {
            string file = this.searchFile.Text;

            var q = from t in context.FileTable
                    from m in context.UserTable
                    where t.FileName.Contains(file) && m.UserName == userName && t.DownloadId == m.UserId
                    select t.FileName;
            this.Filelist.ItemsSource = q.ToList();


        }

        private void decrypt(string priPath,string sessionPath,string filePath)
        {
            string privateKey,sessionKey;
            byte[] enkey, eniv;
            using(StreamReader sr=new StreamReader(sessionPath))
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
            string downFile = this.downloadPath.Text + "\\" + fileName[fileName.Length - 1];
            string decrypt = AesHelp.DescyrptString(filePath, downFile, key, iv);
            using (StreamWriter sw = new StreamWriter(downFile))
            {
                sw.WriteLine(decrypt);
            }
            //this.txt.Text = AesHelp.DescyrptString("../../files/" + userName + "_" + FileName, decrypt, iv);
            //string asd = Convert.ToBase64String(iv);//存储iv
            //this.txt.Text = Convert.ToBase64String(iv);
            //解密key,iv  byte[] ds = Convert.FromBase64String(asd);
        }


        //将用户选中的选项下载并且解密
        private void D_download_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            this.downloadPath.Text = m_Dialog.SelectedPath.Trim();



            string filename,sessionkey,filepath, prikey;
            foreach (var name in this.Filelist.SelectedItems)
            {
                filename = name.ToString();
                var q = from t in context.FileTable
                        from m in context.UserTable
                        where t.FileName == filename && m.UserName == userName 
                        && t.DownloadId == m.UserId
                        select new { t.FilePath,t.SessionKey,m.PrivateKey};
                foreach (var item in q)
                {
                    filepath = item.FilePath;
                    sessionkey = item.SessionKey;
                    prikey = @"../../PrivateKey/" + item.PrivateKey;
                    decrypt(prikey, sessionkey, filepath);
                }
            }
            
            
            
            /**
            //TransmitFile实现下载
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=z.zip");
            string filename = Server.MapPath("DownLoad/z.zip");
            Response.TransmitFile(filename);
            **/
        }


        /***
        /// <summary>
        /// 使用微软的TransmitFile下载文件
        /// </summary>
        /// <param name="filePath">服务器相对路径</param>
        public void TransmitFile(string filePath)
        {
            try
            {
                filePath = Server.MapPath(filePath);
                if (File.Exists(filePath))
                {
                    FileInfo info = new FileInfo(filePath);
                    long fileSize = info.Length;
                    HttpContext.Current.Response.Clear();

                    //指定Http Mime格式为压缩包
                    HttpContext.Current.Response.ContentType = "application/x-zip-compressed";

                    // Http 协议中有专门的指令来告知浏览器, 本次响应的是一个需要下载的文件. 格式如下:
                    // Content-Disposition: attachment;filename=filename.txt
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(info.FullName));
                    //不指明Content-Length用Flush的话不会显示下载进度   
                    HttpContext.Current.Response.AddHeader("Content-Length", fileSize.ToString());
                    HttpContext.Current.Response.TransmitFile(filePath, 0, fileSize);
                    HttpContext.Current.Response.Flush();
                }
            }
            catch
            { }
            finally
            {
                HttpContext.Current.Response.Close();
            }

        }
        ***/
    }
}
