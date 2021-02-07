using GraduationDesign_1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraduationDesign_1
{
    /// <summary>
    /// Upload.xaml 的交互逻辑
    /// </summary>
    public partial class Upload : Window
    {
        private string FilePath;
        private string FileName;
        public string userName { get; set; }

        public Upload()
        {
            InitializeComponent();
        }
        private void showUsers()
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
            this.downloadComo.Items.Clear();
            this.downloadComo.ItemsSource = q.ToList();
        }
        //判断需要使用的功能
        private void choseFunc()
        {
            ComboBoxItem item = this.uploadComo.SelectedItem as ComboBoxItem;
            string function = item.Content.ToString();
            this.downloadComo.ItemsSource = null;
            if (function == "单一用户存储文件")
            {
                ComboBoxItem item1 = new ComboBoxItem();
                item1.Content = userName;
                this.downloadComo.Items.Add(item1);
            }
            else if(function == "用户间传递数据")
            {
                showUsers();
            }
        }
        //选择所要上传的文件
        private void choseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            FileTable file = new FileTable();

            //打开文件选择器，并选择文件
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == false) return;
            //记录文件所处路径，并显示文件名
            FilePath = dialog.FileName;
            file.FilePath = dialog.FileName;
            string[] fileName = FilePath.Split('\\');
            FileName = fileName[fileName.Length - 1];
            file.FileName = FileName;
            this.fileName.Text = FileName;
        }

        private void uploadComo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            choseFunc();
        }

        private void U_upload_Click(object sender, RoutedEventArgs e)
        {
            byte[] key, iv;
            //加密文件内容
            AesHelp.GenKeyIV(out key, out iv);
            AesHelp.EncryptString(FilePath, "../../files/" + userName + "_" + FileName, key, iv);
            saveFile(key, iv);
        }
        //存储文件
        private void saveFile(byte[] key, byte[] iv)
        {
            FileTable file = new FileTable();
            file.FileName = FileName;
            file.FilePath = FilePath;
            file.SessionKey = saveSessionKey(key, iv);

            using (var context = new MyDbEntities())
            {
                var q = from t in context.UserTable
                        where t.UserName == userName
                        select t.UserId;
                file.UploadId = q.First();
                if (this.uploadComo.SelectedIndex == 0)
                    file.DownloadId = q.First();
                else if (this.uploadComo.SelectedIndex == 1)
                    file.DownloadId = this.downloadComo.SelectedIndex;
                try
                {
                    context.FileTable.Add(file);
                    int i = context.SaveChanges();
                    MessageBox.Show("success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private string saveSessionKey(byte[] key, byte[] iv)
        {
            string publicKey, keyPath, sessionKey;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string pubPath = @"../../PublicKey/" + userName + "_publicKey.xml";
            using (StreamReader sr = new StreamReader(pubPath))
            {
                publicKey = sr.ReadLine();
            }
            rsa.FromXmlString(publicKey);
            byte[] keySession = rsa.Encrypt(key, false);
            byte[] ivSession = rsa.Encrypt(iv, false);
            FileName = FileName.Split('.')[0] ;
            keyPath = @"../../SessionKey/" + userName +"_"+FileName+ "_RSAkey.xml";
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
            //private void decrypt()
            //{
            //using (StreamReader sr = new StreamReader(priPath))
            //{
            //    privateKey = sr.ReadLine();
            //}
            //    //解密文件内容
            //    rsa.FromXmlString(privateKey);
            //    byte[] decrypt = rsa.Decrypt(encrypt, false);
            //    //this.txt.Text = AesHelp.DescyrptString("../../files/" + userName + "_" + FileName, decrypt, iv);
            //    string asd = Convert.ToBase64String(iv);//存储iv
            //    this.txt.Text = Convert.ToBase64String(iv);
            //    //解密key,iv  byte[] ds = Convert.FromBase64String(asd);
            //}

        }
}
