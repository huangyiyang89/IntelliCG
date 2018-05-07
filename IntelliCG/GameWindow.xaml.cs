using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using IntelliCG.Cheat;
using IntelliCG.Extensions;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32.SafeHandles;


namespace IntelliCG
{
    public partial class GameWindow
    {


        public GameWindow(CrossGate cg)
        {
            InitializeComponent();
            Cg = cg;
            
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            MainWindow.GameWindows.Add(this);
            
            RegisterTask(Cg.AutoCombat,SwitchZiDongZhanDou);
            RegisterTask(Cg.AutoWalk,SwitchZiDongYuDi);
            RegisterTask(Cg.PetCatch,SwitchZiDongZhuaChong);
            RegisterTask(Cg.Script, SwitchXinCunJiaoBen);
            RegisterTask(Cg.Producer,SwitchZiDongShengChan);
            RegisterTask(Cg.PetCatch,SwitchZiDongZhuaChong);
            RegisterTask(Cg.Poster,SwitchZiDongYouJi);
            RegisterTask(Cg.AutoCure,SwitchZiDongZhiLiao);
            RegisterTask(Cg.AutoFood,SwitchZiDongLiaoLi);
            RegisterTask(Cg.AutoChange,SwitchZiDongHuan);
            
            
            SwitchGaoSuFangShi.Click += (s, e) =>
            {
                Cg.Cheater.GaoSuZhanDou=SwitchGaoSuFangShi.IsChecked.GetValueOrDefault();
            };
            
            
            RefreshSwitch();
            Cg.Cheater.CaiJiJiaSu = true;
            Cg.AutoNurse.Start();
            
        }

        public CrossGate Cg { get; }
        public UnitsWindow UnitsWindow { get; set; }
        
        private void RegisterTask(TaskBase script,ToggleButton switchButton)
        {
            script.Info += On_Info;
            switchButton.Checked += (s, e) =>
            {
                Console.WriteLine(@"Checked");
                script.Start();
            };
            switchButton.Unchecked += (s, e) =>
            {
                Console.WriteLine(@"UnChecked");
                script.Stop();
            };
            script.Stopped += (s, e) => switchButton.IsChecked = false;
            Closed += (s, e) => script.Stop();
        }
       
        private readonly DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        
        //战斗料理
        private void Switch3_Click(object sender, RoutedEventArgs e)
        {
            Cg.Cheater.ZhanDouLiaoLi = SwitchZhanDouLiaoLi.IsChecked.GetValueOrDefault();
        }
       
        //二动吃药
        private void Switch5_Click(object sender, RoutedEventArgs e)
        {
            Cg.Cheater.ErDongJiNeng = SwitchErDongChiYao.IsChecked.GetValueOrDefault();
        }
        
        //允许吃喝
        private void Switch7_Click(object sender, RoutedEventArgs e)
        {
            Cg.AutoCombat.EnableItems = SwitchYunXuChiHe.IsChecked.GetValueOrDefault();
        }
        //烧一技能
        private void Switch8_Click(object sender, RoutedEventArgs e)
        {
            Cg.AutoCombat.AlwaysCastFirstSpell = SwitchShaoYiJiNeng.IsChecked.GetValueOrDefault();
        }
        
        //给宠物吃
        private void Switch10_Click(object sender, RoutedEventArgs e)
        {
            Cg.AutoCombat.EnableFeedPet = SwitchChongWuChiHe.IsChecked.GetValueOrDefault();
        }
        
        //显血
        private void Switch12_Click(object sender, RoutedEventArgs e)
        {
            if (SwitchXianXueChuangKou.IsChecked.GetValueOrDefault())
            {
                
               UnitsWindow=new UnitsWindow(Cg);
               UnitsWindow.Show();
               UnitsWindow.Closed += (se, ev) => { SwitchXianXueChuangKou.IsChecked = false; };

            }
            else
            {
                UnitsWindow?.Close();
            }
        }

        
        private void GameWindow_OnClosed(object sender, EventArgs e)
        {
            _timer.Stop();
            Console.WriteLine(@"GameWindow.Close()");
            Cg.Close();
            UnitsWindow?.Close();
            MainWindow.GameWindows.Remove(this);
        }

        private void On_Info(object sender,EventArgs e)
        {
            RichTextBox.AppendText(((MessageEventArgs)e).Info + "\r\n");
        }
        
        public void MoveGameWindowAndSelf(int x, int y)
        {
            var dpiFactor = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11;


            Cg.Memo.MoveWindow(Convert.ToInt32(x * dpiFactor), Convert.ToInt32(y * dpiFactor));
            FollowGameWindow();
            
            
        }
        public void FollowGameWindow()
        {
           //646,509
           var dpiFactor = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11;
           var rect = Cg.Memo.GetWindowRect();
           Left = rect.Left / (dpiFactor??1)+3;
           Top=rect.Bottom / (dpiFactor??1)-2;
           Title = Cg.Player.Name;
        }
        
        private async void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            var reuslt=await this.ShowMessageAsync("告诉你不要点了", "强制关闭游戏，可以断线重连，是否继续？", MessageDialogStyle.AffirmativeAndNegative);
            if (reuslt == MessageDialogResult.Affirmative)
            {
                Cg.Memo.Process.Kill();
            }
        }

        
        private void Button2_OnClick(object sender, RoutedEventArgs e)
        {
            var s=Cg.Stuffs.FindXiuGe();
            

        }
        private void RefreshSwitch()
        {
            SwitchGaoSuFangShi.IsChecked = Cg.Cheater.GaoSuZhanDou;
            SwitchZhanDouLiaoLi.IsChecked = Cg.Cheater.ZhanDouLiaoLi;
            SwitchErDongChiYao.IsChecked = Cg.Cheater.ErDongJiNeng;
        }

        private  void Timer_Tick(object sender, EventArgs e)
        {
            
            if (Cg.Memo.Process.HasExited)
            {
                Close();
            }
            else
            {
                FollowGameWindow();
            }
        }
        
        private void RichTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox.ScrollToEnd();
            
        }

        private void ComboDengDaiShiJiang_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Cg != null)
            {
                Cg.AutoCombat.GaoSuDelay = ComboDengDaiShiJiang.SelectedIndex > -1
                    ? (ComboDengDaiShiJiang.SelectedIndex + 4) * 1000
                    : 4000;
            }
        }

        private void SwitchXueShaoTingZhi_OnClick(object sender, RoutedEventArgs e)
        {
            Cg.AutoWalk.StopOnHp = SwitchXueShaoTingZhi.IsChecked.GetValueOrDefault();
        }
    }
}
