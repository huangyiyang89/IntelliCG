using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dm;
using IntelliCG.Combat;
using MahApps.Metro.Controls;

namespace IntelliCG
{
    public partial class GameWindow : MetroWindow
    {

        
        public GameWindow(CrossGate cg)
        {
            InitializeComponent();
            CrossGate = cg;
            AutoCombat = new AutoCombat(cg);
            MainWindow.GameWindows.Add(this);
            FollowGame();
            AutoCombat.Infomation += new AutoCombat.MessageHandler(OnInfo);

        }
        

        public CrossGate CrossGate { get; }
        public AutoCombat AutoCombat { get; }

        //高速战斗
        private void Switch1_Click(object sender, RoutedEventArgs e)
        {
            
            CrossGate.Cheat.GaoSuZhanDou = Switch1.IsChecked.GetValueOrDefault();
        }
       
        // 步步遇敌
        private void Switch2_Click(object sender, RoutedEventArgs e)
        {
            CrossGate.Cheat.BuBuYuDi = Switch2.IsChecked.GetValueOrDefault();
        }
        //战斗料理
        private void Switch3_Click(object sender, RoutedEventArgs e)
        {
            CrossGate.Cheat.ZhanDouLiaoLi = Switch3.IsChecked.GetValueOrDefault();
        }
        //移动加速
        private void Switch4_Click(object sender, RoutedEventArgs e)
        {
            CrossGate.Cheat.YiDongJiaSu = Switch4.IsChecked.GetValueOrDefault();
        }
        //采集加速
        private void Switch5_Click(object sender, RoutedEventArgs e)
        {
            CrossGate.Cheat.CaiJiJiaSu = Switch5.IsChecked.GetValueOrDefault();
        }
        //自动战斗
        private void Switch6_Click(object sender, RoutedEventArgs e)
        {
            AutoCombat.EnableAutoCombat = Switch6.IsChecked.GetValueOrDefault();
        }

        //允许吃喝
        private void Switch7_Click(object sender, RoutedEventArgs e)
        {
            AutoCombat.EnableItems = Switch7.IsChecked.GetValueOrDefault();
        }

        //烧一技能
        private void Switch8_Click(object sender, RoutedEventArgs e)
        {
            AutoCombat.AlwaysFirstSpell = Switch8.IsChecked.GetValueOrDefault();
        }
        //自动遇敌
        private void Switch9_Click(object sender, RoutedEventArgs e)
        {
            AutoCombat.EnableAutoWalk = Switch9.IsChecked.GetValueOrDefault();
        }



        private void GameWindow_OnClosed(object sender, EventArgs e)
        {
            AutoCombat.EnableAutoCombat = false;
            MainWindow.GameWindows.Remove(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CrossGate.Combat.Units.Read();
            Console.WriteLine();

        }

        private  void OnInfo(string info)
        {
            this.Invoke(() => RichTextBox.AppendText(info+"\r\n"));
        }


        public void FollowGame()
        {
            var scale= (CrossGate.Window.X2-CrossGate.Window.X1)/640;
            Left = CrossGate.Window.X3/scale;
            Top = CrossGate.Window.Y3/scale;
            Title = CrossGate.Player.Name;
        }
        
    }
}
