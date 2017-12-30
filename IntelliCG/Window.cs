using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntelliCG
{
    public class Window : Base
    {

        public Window(int hwnd) : base(hwnd)
        {
        }

        public double X1
        {
            get
            {
                Dm.GetWindowRect(Hwnd, out var x1, out var y1, out var x4, out var y4);
                return Convert.ToDouble(x1);
            }
        }

        public double X2 => X4;

        public double X3 => X1;

        public double X4
        {
            get
            {
                Dm.GetWindowRect(Hwnd, out var x1, out var y1, out var x4, out var y4);
                return Convert.ToDouble(x4);
            }
        }

        public double Y1
        {
            get
            {
                Dm.GetWindowRect(Hwnd, out var x1, out var y1, out var x4, out var y4);
                return Convert.ToDouble(y1);
            }
        }
        public double Y2 => Y1;
        public double Y3 => Y4;

        public double Y4
        {
            get
            {
                Dm.GetWindowRect(Hwnd, out var x1, out var y1, out var x4, out var y4);
                return Convert.ToDouble(y4);
            }
        }

        public static List<Window> FindAllWindows()
        {
            var windows = new List<Window>();
            var hwndStrs = Dm.EnumWindow(0, "", "魔力宝贝", 1 + 2);
            if (hwndStrs == "")
            {
                return windows;
            }
            var hwnds = hwndStrs.Split(',');
            windows.AddRange(hwnds.Select(hwnd => new Window(Convert.ToInt32(hwnd))));
            return windows;
        }

        public static void TileWindows(List<Window> windows)
        {
            

            var screenWidth = Convert.ToInt32(SystemParameters.PrimaryScreenWidth);
            if (SystemParameters.PrimaryScreenWidth > 2500)
            {
                for (var i = 0; i < windows.Count; i++)
                {
                    var j = i / 4;
                    windows[i].MoveTo(960 * (i%4), j * 200);
                }
            }
            else if (SystemParameters.PrimaryScreenWidth > 1900)
            {
                for (var i = 0; i < windows.Count; i++)
                {
                    var j = i / 3;
                    windows[i].MoveTo(640 * (i % 4), j * 150);
                }
            }
            else if (SystemParameters.PrimaryScreenWidth > 1300)
            {
                
                for (var i = 0; i < windows.Count; i++)
                {
                    switch (i)
                    {
                        case 1:
                            windows[i].MoveTo((screenWidth - 640) / 2, 0);
                            break;
                        case 2:
                            windows[i].MoveTo(screenWidth - 640, 0);
                            break;
                        case 3:
                            windows[i].MoveTo(0, 100);
                            break;
                        case 4:
                            windows[i].MoveTo((screenWidth - 640) / 2, 100);
                            break;
                        case 5:
                            windows[i].MoveTo(screenWidth - 640, 100);
                            break;
                        default:
                            windows[i].MoveTo(0, 0);
                            break;
                    }
                }
            }





        }

        public bool IsExist => Dm.GetWindowState(Hwnd, 0) == 1;

        public void MoveTo(int x, int y)
        {
            Dm.MoveWindow(Hwnd, x, y);
        }

        
    }
}
