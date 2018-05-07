using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Cheat
{
    public class Actions : Base
    {
        public Actions(Memo memo) : base(memo)
        {
        }

        public async Task MoveTo(int x, int y)
        {
            Memo.WriteBytes(0x0048940D, "B8-00-00-00-00");
            Memo.WriteBytes(0x00489431, "BA-00-00-00-00-90");
            Memo.WriteInt(0x0048940E, x);
            Memo.WriteInt(0x00489432, y);
            Memo.WriteInt(0x00CB89B0, 1);
            
            Console.WriteLine($@"Moveto {x},{y}");
            await Task.Delay(100);
            Memo.WriteInt(0x00CB89B0, 0);
            Memo.WriteBytes(0x0048940D, "A1-90-89-CB-00");
            Memo.WriteBytes(0x00489431, "8B 15 88 89 CB 00");
            await Task.Delay(500);
        }

       

        public async Task SendCall(string content)
        {
            Memo.WriteBytes(0x00D5A050, content);
            var lenth = content.Split(' ').Length.ToString("x8");
            var code = " " + lenth.Substring(6, 2) + " " + lenth.Substring(4, 2) + " " + lenth.Substring(2, 2) + " " +
                       lenth.Substring(0, 2) + " ";
            Memo.WriteBytes(0x00D5A000, "6A 00 68" + code + "68 50 A0 D5 00 FF 35 20 08 E1 00 E8 C7 AB 80 FF C3");
            await Task.Delay(100);
            Memo.RemoteCall(0x00D5A000);
            //call地址 00518B75
            ////缓存地址 00D57F68
            //Dm.WriteData(Hwnd, "00D5A000", content);
            //var len = content.Split(' ').Length;
            //Dm.AsmClear();
            //Dm.AsmAdd("push 00");
            //Dm.AsmAdd("push " + "0" + len.ToString("X"));
            //Dm.AsmAdd("push 00D5A000");
            //Dm.AsmAdd("push [00E10820]");
            //Dm.AsmAdd("call 00564BDE");
            //Dm.AsmCall(Hwnd, 1);
        }


        //第一格第二格番茄酱 -C6 0 0 -1 909|8|9 
        public async Task DecodeSend(string content)
        {
            Console.WriteLine(content);
            Memo.WriteBytes(0x00D5A000, "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00");
            Memo.WriteString(0x00D5A000, content);
            Memo.WriteBytes(0x0054F0B3, "B8-00-A0-D5-00");
            await RightClick(0, 0);
            //改回
            Memo.WriteBytes(0x0054F0B3, "A1-A0-04-63-00");
        }


        public async Task RightClick(int x, int y)
        {

            Memo.WriteBytes(0x00402E09, "90 90 90 90 90");
            Memo.WriteBytes(0x00402E1E, "90 90 90 90 90 90");
            Memo.WriteBytes(0x00402E61, "90 90 90 90 90 90");
            Memo.WriteInt(0x00CB8990, x);
            Memo.WriteInt(0x00CB8988, y);
            Memo.WriteInt(0x00CB89BC, 1);
            await Task.Delay(30);

            //还原
            Memo.WriteBytes(0x00402E09, "A3 88 89 CB 00");
            Memo.WriteBytes(0x00402E1E, "89 0D 90 89 CB 00");
            Memo.WriteBytes(0x00402E61, "89 3D BC 89 CB 00");
            Memo.WriteInt(0x00CB89BC, 0);

        }
    }
}
