using GraduationDesign_1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Registration.xaml 的交互逻辑
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
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
                    select t;
            //step 3
            //this.dataGrid.ItemsSource = q.ToList();
        }
        private void R_Submit_Click(object sender, RoutedEventArgs e)
        {
            if(this.R_Name.Text=="" || this.R_password.Text == "" || this.R_password_Confirm.Text=="")
            {
                MessageBox.Show("用户名或密码不能为空！");
            }else if(this.R_password.Text != this.R_password_Confirm.Text)
            {
                MessageBox.Show("密码不一致！请重新输入！");
                this.R_password.Text = "";
                this.R_password_Confirm.Text = "";
            }
            else
            {   
                UserTable user = new UserTable();
                user.UserName = this.R_Name.Text;
                user.UserPasswd = this.R_password.Text;
                user.PrivateKey = this.R_Name.Text+ "_privateKey.xml";
                user.PublicKey = this.R_Name.Text + "_publicKey.xml";
                CreateKey(this.R_Name.Text);
                //showUsers();
                
                using (var context = new MyDbEntities())
                {
                    try
                    {
                        context.UserTable.Add(user);
                        int i = context.SaveChanges();
                        MessageBox.Show("success!");
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("fail!");
                    }
                }
                
            }
        }
        //创建RSA公私钥对
        private void CreateKey(string userName)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string priPath = @"../../PrivateKey/" + this.R_Name.Text + "_privateKey.xml";
            string pubPath = @"../../PublicKey/" + this.R_Name.Text + "_publicKey.xml";

            FileStream fs1 = new FileStream(priPath, FileMode.Create,FileAccess.Write);
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
    }
}
