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
    /// IndexWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IndexWindow : Window
    {
        public string userName { get; set; }
        public IndexWindow()
        {
            InitializeComponent();
        }

        private void M_Upload_Click(object sender, RoutedEventArgs e)
        {
            Upload upload = new Upload();
            upload.userName = userName;
            upload.Show();
        }

        private void M_Download_Click(object sender, RoutedEventArgs e)
        {
            Download download = new Download(userName);
            download.Show();
        }
    }
}
