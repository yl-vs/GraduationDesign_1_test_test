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
        public MyDbEntities context = new MyDbEntities();

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

        private int getID(string name)
        {
            var q = from t in context.UserTable
                    where t.UserName == name
                    select t.UserId;
            return q.First();
        }

        private void saveFile(FileTable file)
        {
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
            ComboBoxItem item = this.uploadComo.SelectedItem as ComboBoxItem;
            string function = item.Content.ToString();
            this.downloadComo.ItemsSource = null;
            if (function == "单一用户存储文件")
            {
                ComboBoxItem item1 = new ComboBoxItem();
                item1.Content = userName;
                this.downloadComo.Items.Add(item1);
            }
            else if (function == "用户间传递数据")
            {
                showUsers();
            }
        }

        private void U_upload_Click(object sender, RoutedEventArgs e)
        {
            byte[] key, iv;
            //加密文件内容
            AesHelp.GenKeyIV(out key, out iv);
            AesHelp.EncryptString(FilePath, "../../files/" + userName + "_" + FileName, key, iv);
            createFile(key, iv);
        }
        //存储文件
        private void createFile(byte[] key, byte[] iv)
        {
            FileTable file = new FileTable();
            file.FileName = FileName;
            file.FilePath = "../../files/" + userName + "_" + FileName;
            


            if (this.uploadComo.SelectedIndex == 0)
            {
                file.SessionKey = saveSessionKey(key, iv,userName);
                file.DownloadId = getID(userName);
                saveFile(file);
            }
            else if (this.uploadComo.SelectedIndex == 1)
            {
                foreach (var item in this.downloadComo.SelectedItems)
                {
                    file.DownloadId = getID(item.ToString());
                    file.SessionKey = saveSessionKey(key, iv, item.ToString());
                    saveFile(file);
                }
            }

        }
        //加密并保存会话密钥
        private string saveSessionKey(byte[] key, byte[] iv,string name)
        {
            string publicKey, keyPath, sessionKey;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //导入下载方RSA公钥
            string pubPath = @"../../PublicKey/" + name + "_publicKey.xml";
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
            keyPath = @"../../SessionKey/" + name + "_" + FileName + "_RSAkey.xml";
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
    }
}
