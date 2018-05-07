using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Scripts
{
    public class AutoNurse:TaskBase
    {

        private int NurseType => Memo.ReadInt(0x00D4A9AC);

        private bool IsOpenningNurseWindow()
        {
            //006130FC =2 加血窗口
            var openNurse= Memo.ReadInt(0x006130FC)==2;
            return openNurse && (NurseType == 328 || NurseType == 364);
        }
        
        private async Task CallNurse()
        {
            //修改内存保护
            Memo.ChangeProtect(0x00D5A000);
            Memo.ChangeProtect(0x004027F2);
            
            //写入代码
            Memo.WriteBytes(0x00D5A000, "A1 B0 A9 D4 00 8D 15 FC 9F D5 00 52 6A 04 50 68 4C 01 00 00 B9 E4 03 CB 00 E8 42 81 7E FF 50 B9 B4 03 CB 00 E8 37 81 7E FF 8B 15 20 08 E1 00 50 A1 0C F9 0A 01 68 90 B8 5C 00 52 50 E8 0F FA 72 FF 68 F0 00 00 00 68 40 01 00 00 6A 4F C7 05 FC 30 61 00 FF FF FF FF E8 E4 C5 7D FF 68 F0 00 00 00 68 40 01 00 00 6A 39 E8 D3 C5 7D FF 83 C4 3C A1 B0 A9 D4 00 8D 15 FC 9F D5 00 52 6A 04 50 68 4C 01 00 00 B9 E4 03 CB 00 E8 D2 80 7E FF 50 B9 B4 03 CB 00 E8 C7 80 7E FF 8B 15 20 08 E1 00 50 A1 0C F9 0A 01 68 90 B8 5C 00 52 50 E8 9F F9 72 FF 68 F0 00 00 00 68 40 01 00 00 6A 4F C7 05 FC 30 61 00 FF FF FF FF E8 74 C5 7D FF 68 F0 00 00 00 68 40 01 00 00 6A 39 E8 63 C5 7D FF 83 C4 3C A1 B0 A9 D4 00 8D 15 FC 9F D5 00 52 6A 04 50 68 4C 01 00 00 B9 E4 03 CB 00 E8 62 80 7E FF 50 B9 B4 03 CB 00 E8 57 80 7E FF 8B 15 20 08 E1 00 50 A1 0C F9 0A 01 68 90 B8 5C 00 52 50 E8 2F F9 72 FF 68 F0 00 00 00 68 40 01 00 00 6A 4F C7 05 FC 30 61 00 FF FF FF FF E8 04 C5 7D FF 68 F0 00 00 00 68 40 01 00 00 6A 39 E8 F3 C4 7D FF 83 C4 3C C7 05 F2 27 40 00 F6 45 0C 02 C7 05 F6 27 40 00 74 6B 39 05 E9 FA 86 6A FF 90");
            //普通护士328 资深护士364
            if (NurseType == 328)
            {
                Memo.WriteBytes(0x00D5A010, "49");
                Memo.WriteBytes(0x00D5A080, "4B");
                Memo.WriteBytes(0x00D5A0F0, "4C");
            }
            else
            {
                Memo.WriteBytes(0x00D5A010, "6D");
                Memo.WriteBytes(0x00D5A080, "6F");
                Memo.WriteBytes(0x00D5A0F0, "70");
            }
            //修改跳转
            Memo.WriteBytes(0x004027F2, "E9 09 78 95 00 90 39 05");
            await Task.Delay(300);
            //300毫秒未执行，跳转主动改回
            Memo.WriteBytes(0x004027F2, "F6 45 0C 02 74 6B 39 05");

        }
        
        public override async Task DoOnce()
        {
            if (IsOpenningNurseWindow())
            {
                await CallNurse();
                await Task.Delay(1000);
            }
        }

        public AutoNurse(CrossGate cg) : base(cg)
        {
        }
    }

    
}
