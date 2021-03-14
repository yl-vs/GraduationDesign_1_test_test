using GraduationDesign_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        MyDbEntities context = new MyDbEntities();
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(this.R_Name.Text == "" || this.R_password.Text == "")
            {
                MessageBox.Show("用户名或密码不能为空！");
            }
            else
            {
                var q = from t in context.UserTable
                        where t.UserName == this.R_Name.Text && t.UserPasswd == this.R_password.Text
                        select t;
                if (q.Count() == 0)
                    MessageBox.Show("用户名或密码不正确！");
                else
                {
                    Upload upload = new Upload();
                    upload.userName = this.R_Name.Text;
                    upload.Show();

                    //下载
                    //1.通过UserName查到UserId
                    var p = from t in context.UserTable
                            where t.UserName == this.R_Name.Text
                            select t.UserId;

                    Download download = new Download();
                    download.D_downloadid = (TextBox)p;
                }
            }
        }
    }
}
