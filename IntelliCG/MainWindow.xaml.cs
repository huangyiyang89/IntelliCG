using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Dm;
using IntelliCG.Combat;
using MahApps.Metro.Controls;
using Application = System.Windows.Application;

namespace IntelliCG
{
   
    public partial class MainWindow : MetroWindow
    {
        public static List<GameWindow> GameWindows { get; } =new List<GameWindow>();
        private readonly NotifyIcon _notifyIcon;
        
        
        public MainWindow()
        {
           
            InitializeComponent();
            Hide();
            

            var contextMenu = new ContextMenu();
            
            contextMenu.MenuItems.Clear();
            
            var menuItemRefresh = new MenuItem("Refresh");
            menuItemRefresh.Click += MenuItemRefresh_Click;
            contextMenu.MenuItems.Add(menuItemRefresh);

            var menuItemExit = new MenuItem("Exit");
            menuItemExit.Click += MenuItemExit_Click;
            contextMenu.MenuItems.Add(menuItemExit);
            

            _notifyIcon = new NotifyIcon
            {
                BalloonTipTitle = @"IntelliCG",
                BalloonTipText = @"IntelliCG is running here, double click to refresh.", //设置程序启动时显示的文本
                BalloonTipIcon = ToolTipIcon.Info,
                Text = @"IntelliCG",//最小化到托盘时，鼠标点击时显示的文本
                Icon = new Icon("icon.ico"),//程序图标
                Visible = true,
        };
            _notifyIcon.ShowBalloonTip(0);
            _notifyIcon.MouseDoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.ContextMenu = contextMenu;
            
            Refresh();
        }

        

        private static void MenuItemExit_Click(object sender, EventArgs e)
        {
            foreach(var window in GameWindows)
            {
                window.AutoCombat.EnableAutoCombat = false;
            }
           Application.Current.Shutdown();
        }

        private void MenuItemRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            //Show();
            //WindowState = WindowState.Normal;
            //_notifyIcon.Visible = false;
            Refresh();
            
        }
        
        private void Refresh()
        {

            
            
            var windows = Window.FindAllWindows();

            Window.TileWindows(windows);

            //如果窗口不在打开新窗口
            foreach (var window in windows)
            {
                //窗口中不包含新发现的游戏
                if (GameWindows.Exists(w => w.CrossGate.Window.Hwnd == window.Hwnd)) continue;
                var newGame = new CrossGate(window.Hwnd);
                new GameWindow(newGame).Show();
            }

            //关闭游戏不存在的窗口
            for (var i = GameWindows.Count - 1; i >= 0; i--)
            {
                if (GameWindows[i].CrossGate.Window.IsExist)
                {
                    GameWindows[i].FollowGame();
                }
                else
                {
                    GameWindows[i].Close();
                }
            }

            _notifyIcon.BalloonTipText = @"Game Binding Refreshed.";
            _notifyIcon.ShowBalloonTip(0);
        }

        
    }
}

