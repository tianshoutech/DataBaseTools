using DataBaseTools.Common;
using DataBaseTools.Common.Model;
using DataBaseTools.Model;
using DataBaseTools.UI.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DataBaseTools.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        public void Init()
        {
            // TODO:此处应该放置在应用的初始化代码中
            ResourceManager.SetDefaultResource("DataBaseTools.UI.Languages.lang", "DataBaseTools.UI");

            this.Title = ResourceManager.GetResourse("MainPageTitle");
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeDBMenu_Click(object sender, RoutedEventArgs e)
        {
            var changDBPage = new ChangDBPage();
            changDBPage.SqlTypeListCbox.ItemsSource = CommonConst.SqlTypeList;
            changDBPage.SqlTypeListCbox.DisplayMemberPath = "SqlName";
            changDBPage.SqlTypeListCbox.SelectedValuePath = "Id";
            if (CommonConst.SqlTypeList.Count > 0)
            {
                changDBPage.SqlTypeListCbox.SelectedIndex = 0;
            }
            changDBPage.ShowDialog();
        }

        private void testRedisBtn_Click(object sender, RoutedEventArgs e)
        {
            //var keys = RedisManager.GetAllKeys().ToList();
            //var infos = RedisManager.GetInfo();
            var list = new List<RedisStringModel>();
            for (int i = 0; i < 1000000; i++)
            {
                list.Add(new RedisStringModel()
                {
                    key = "name"+i,
                    Value = "name" + i,
                    ExpiredTime = 100000
                });
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            RedisManager.MSetValue(list);
            sw.Stop();
            MessageBox.Show((sw.ElapsedMilliseconds / 1000).ToString());
        }
    }
}
