//    using System;
//    using System.Collections.Generic;
    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using System.Windows;
//    using System.Windows.Controls;
//    using System.Windows.Data;
//    using System.Windows.Documents;
//    using System.Windows.Input;
//    using System.Windows.Media;
//    using System.Windows.Media.Imaging;
//    using System.Windows.Shapes;



using GraduationDesign_1.Models;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.IO;





namespace GraduationDesign_1
{
    /// <summary>
    /// Download.xaml 的交互逻辑
    /// </summary>
    public partial class Download : Window
    {
        private object listBox;
        MyDbEntities context = new MyDbEntities();


        public Download()
        {
            InitializeComponent();
        }

        //将用户选中的选项删除同时需要删除数据库中的相应数据段
        private void D_clear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = Filelist.SelectedItems.Count -1;i>=0;i--)
            {
                string s = (string)Filelist.SelectedItems[i];
                //删除filelist中的选定项
                Filelist.Items.Remove(Filelist.SelectedItems[i]);
                //删除数据库中的数据项
                var q = from t in context.FileTable
                        where t.FilePath == s
                        select t;
                foreach(var v in q)
                {
                    context.FileTable.Remove(v);
                }
            }

        }

        //将数据库中用户为接收方的文件路径检索到listbox中
        private void D_retrieve_Click(object sender, RoutedEventArgs e)
        {
            string text = D_downloadid.Text;
            int id = int.Parse(text);
            var q = from t in context.FileTable
                    where t.DownloadId == id
                    select t.FilePath;
            Filelist.Items.Add(q);
        }



        //将用户选中的选项下载并且解密
        private void D_download_Click(object sender, RoutedEventArgs e)
        {
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
