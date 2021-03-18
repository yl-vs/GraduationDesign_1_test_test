using GraduationDesign_1.CloudServiceReference;
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
        
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.R_Name.Text == "" || this.R_password.Text == "")
            {
                MessageBox.Show("用户名或密码不能为空！");
            }
            else
            {
                CryptoServiceClient client = new CryptoServiceClient();
                int c = client.Login(this.R_Name.Text, this.R_password.Text);
                if (c == 0)
                    MessageBox.Show("用户名或密码不正确！");
                else
                {
                    IndexWindow indexWindow = new IndexWindow();
                    indexWindow.userName = this.R_Name.Text;
                    indexWindow.Show();
                    this.Close();
                }
            }
        }
    }
}
