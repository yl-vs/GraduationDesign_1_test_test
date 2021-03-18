﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraduationDesign_1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void M_login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            //IndexWindow indexWindow = new IndexWindow();
            //indexWindow.userName = "安好";
            //indexWindow.Show();
            this.Close();
        }

        private void M_Registration_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
        }
    }
}
