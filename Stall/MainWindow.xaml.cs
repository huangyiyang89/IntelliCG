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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CgData;

namespace Stall
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ss=new StallService();
            var s = new CgData.Models.Stall()
            {
                X = 110,
                Y = 234,
                Line = "II",
                Description = "dsasdsasd",
                PlayerName = "dsafgggwwww"
            };
            ss.AddStall(s);

        }
    }
}
