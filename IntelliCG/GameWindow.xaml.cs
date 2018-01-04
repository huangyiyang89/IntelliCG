using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using IntelliCG.Combat;

using MahApps.Metro.Controls;

namespace IntelliCG
{
    public partial class GameWindow : MetroWindow
    {

        
        public GameWindow(CrossGate cg)
        {
            InitializeComponent();
            Cg = cg;
            AutoCombat = new AutoCombat(cg);
            NewTown=Script.Script.GetInstance(cg);
            MainWindow.GameWindows.Add(this);
            AutoCombat.Infomation += AutoCombat_OnInfo;
            RefreshSwitch();
        }
        
        public CrossGate Cg { get; }
        public AutoCombat AutoCombat { get; }

        public Script.Script NewTown { get; }

        //高速战斗
        private void Switch1_Click(object sender, RoutedEventArgs e)
        {
            
            Cg.Cheat.GaoSuZhanDou = Switch1.IsChecked.GetValueOrDefault();
        }
       
        // 步步遇敌
        private void Switch2_Click(object sender, RoutedEventArgs e)
        {
            Cg.Cheat.BuBuYuDi = Switch2.IsChecked.GetValueOrDefault();
        }
        //战斗料理
        private void Switch3_Click(object sender, RoutedEventArgs e)
        {
            Cg.Cheat.ZhanDouLiaoLi = Switch3.IsChecked.GetValueOrDefault();
        }
        //移动加速
        private void Switch4_Click(object sender, RoutedEventArgs e)
        {
            Cg.Cheat.YiDongJiaSu = Switch4.IsChecked.GetValueOrDefault();
        }
        //采集加速
        private void Switch5_Click(object sender, RoutedEventArgs e)
        {
            Cg.Cheat.CaiJiJiaSu = Switch5.IsChecked.GetValueOrDefault();
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
        //给宠物吃
        private void Switch10_Click(object sender, RoutedEventArgs e)
        {
            AutoCombat.EnableFeedPet = Switch10.IsChecked.GetValueOrDefault();
        }

        private void Switch11_Click(object sender, RoutedEventArgs e)
        {
            NewTown.EnableScript = Switch11.IsChecked.GetValueOrDefault();
        }
        private void GameWindow_OnClosed(object sender, EventArgs e)
        {
            Cg.Cheat.BuBuYuDi = false;
            Cg.Cheat.CaiJiJiaSu = false;
            Cg.Cheat.GaoSuZhanDou = false;
            Cg.Cheat.YiDongJiaSu = false;
            Cg.Cheat.ZhanDouLiaoLi = false;

            NewTown.EnableScript = false;
            AutoCombat.EnableAutoCombat = false;
            MainWindow.GameWindows.Remove(this);
        }

        private  void AutoCombat_OnInfo(string info)
        {
            if (info.Contains("停止自动遇敌"))
            {
                this.Invoke(RefreshSwitch);
                Task.Run(() => MessageBox.Show(info));
                return;
            }
            this.Invoke(() => RichTextBox.AppendText(info+"\r\n"));
        }


        public void MoveGameWindowAndSelf(int x,int y)
        {
            var dpiFactor = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            Cg.Memo.MoveWindow(Convert.ToInt32(x*dpiFactor), Convert.ToInt32(y *dpiFactor));
            Left = x+2.5;
            Top = y + 507;
            Title = Cg.Player.Name;
        }

        private void RichTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox.ScrollToEnd();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //东医回蓝 AF EF 18 00 12 1C 4F 4F 4F 54 41 24 59 1D 09 61 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //东医回血 AF EF 18 00 12 1C 4F 4F 4F 54 41 24 59 1D 0F 61 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //东医回宠 AF EF 18 00 12 1C 4F 4F 4F 54 41 24 59 1D 0E 61 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //资深回蓝 AF EF 18 00 1D 1C 4F 4F 4F 54 4F 24 59 1D 37 24 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //资深回血 AF EF 18 00 1D 1C 4F 4F 4F 54 4F 24 59 1D 35 24 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //资深回宠 AF EF 18 00 1D 1C 4F 4F 4F 54 4F 24 59 1D 34 24 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //资深回宠 AF EF 18 00 1C 1C 4F 4F 4F 54 4E 24 59 1D 34 24 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //资深回宠 AF EF 18 00 13 1C 4F 4F 4F 54 4F 24 59 1D 34 24 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF
            //Cg.Call("AF EF 18 00 12 1C 4F 4F 4F 54 41 24 59 1D 0E 61 41 50 21 5B 5A 2B 17 38 38 4C 46 1D EF AF");
            
           // Cg.Memo.Process.Kill();
            //Close();
            //006130FC =2 加血窗口
            //Cg.NurseCall();
            //Cg.RightClick(73,83);
            //Cg.MoveTo(352,196);
            //var s=Script.Script.GetInstance(Cg);
            //s.Run();

        }

        private void RefreshSwitch()
        {
            Switch1.IsChecked = Cg.Cheat.GaoSuZhanDou;
            Switch2.IsChecked = Cg.Cheat.BuBuYuDi;
            Switch3.IsChecked = Cg.Cheat.ZhanDouLiaoLi;
            Switch4.IsChecked = Cg.Cheat.YiDongJiaSu;
            Switch5.IsChecked = Cg.Cheat.CaiJiJiaSu;
            Switch6.IsChecked = AutoCombat.EnableAutoCombat;
            Switch7.IsChecked = AutoCombat.EnableItems;
            Switch8.IsChecked = AutoCombat.AlwaysFirstSpell;
            Switch9.IsChecked = AutoCombat.EnableAutoWalk;
            Switch10.IsChecked = AutoCombat.EnableFeedPet;
            Switch11.IsChecked = NewTown.EnableScript;
        }
    }
}
