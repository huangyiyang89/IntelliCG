using System.Threading;
using System.Threading.Tasks;
using IntelliCG.Item;
using IntelliCG.MemoryHelper;
using IntelliCG.Pet;
using SpellList = IntelliCG.Player.SpellList;

namespace IntelliCG
{
    public class CrossGate:Base
    {
        
        public Player.Player Player { get;}
        public Cheat.Cheat Cheat { get; }

        public ItemList Items { get; }
        
        public Combat.Combat Combat { get;}
        
        public PetList Pets { get; }
        

        public CrossGate(Memo memo):base(memo)
        {
            Cheat = new Cheat.Cheat(memo);
            Player=new Player.Player(memo);
            Combat=new Combat.Combat(memo);
            Items=new ItemList(memo);
            Pets=new PetList(memo);
        }
        
        public void MoveTo(int x, int y)
        {
            Memo.WriteBytes(0x0048940D, "B8-00-00-00-00");
            Memo.WriteBytes(0x00489431, "BA-00-00-00-00-90");
            Memo.WriteInt(0x0048940E, x);
            Memo.WriteInt(0x00489432, y);
            Memo.WriteInt(0x00CB89B0, 1);
            Thread.Sleep(700);
            Memo.WriteInt(0x00CB89B0, 0);
            Memo.WriteBytes(0x0048940D, "A1-90-89-CB-00");
            Memo.WriteBytes(0x00489431, "8B 15 88 89 CB 00");
        }

        public void Call(string content)
        {
            Memo.WriteBytes(0x00D5A050, content);
            var lenth= content.Split(' ').Length.ToString("x8");
            var code = " "+ lenth.Substring(6, 2) +" "+ lenth.Substring(4, 2) +" "+ lenth.Substring(2, 2) +" "+ lenth.Substring(0, 2)+" ";
            Memo.WriteBytes(0x00D5A000, "6A 00 68"+code+"68 50 A0 D5 00 FF 35 20 08 E1 00 E8 C7 AB 80 FF C3");
            Thread.Sleep(100);
            Memo.RemoteCall(0x00D5A000);
            //call地址 00518B75
            ////00D57F68
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

        
        public void NurseCall()
        {
            //006130FC =2 加血窗口
            //普通149  14B    14C    资深16D 16F  170
            var openNurse= Memo.ReadInt(0x006130FC)==2;
            var type = Memo.ReadInt(0x00D4A9AC);
            if (!openNurse || (type != 328 && type != 364))
            {
                return;
            }
            //修改内存保护
            Memo.ChangeProtect(0x00D5A000);
            Memo.ChangeProtect(0x004027F2);
            
            //写入代码
            Memo.WriteBytes(0x00D5A000, "A1 B0 A9 D4 00 8D 15 FC 9F D5 00 52 6A 04 50 68 4C 01 00 00 B9 E4 03 CB 00 E8 42 81 7E FF 50 B9 B4 03 CB 00 E8 37 81 7E FF 8B 15 20 08 E1 00 50 A1 0C F9 0A 01 68 90 B8 5C 00 52 50 E8 0F FA 72 FF 68 F0 00 00 00 68 40 01 00 00 6A 4F C7 05 FC 30 61 00 FF FF FF FF E8 E4 C5 7D FF 68 F0 00 00 00 68 40 01 00 00 6A 39 E8 D3 C5 7D FF 83 C4 3C A1 B0 A9 D4 00 8D 15 FC 9F D5 00 52 6A 04 50 68 4C 01 00 00 B9 E4 03 CB 00 E8 D2 80 7E FF 50 B9 B4 03 CB 00 E8 C7 80 7E FF 8B 15 20 08 E1 00 50 A1 0C F9 0A 01 68 90 B8 5C 00 52 50 E8 9F F9 72 FF 68 F0 00 00 00 68 40 01 00 00 6A 4F C7 05 FC 30 61 00 FF FF FF FF E8 74 C5 7D FF 68 F0 00 00 00 68 40 01 00 00 6A 39 E8 63 C5 7D FF 83 C4 3C A1 B0 A9 D4 00 8D 15 FC 9F D5 00 52 6A 04 50 68 4C 01 00 00 B9 E4 03 CB 00 E8 62 80 7E FF 50 B9 B4 03 CB 00 E8 57 80 7E FF 8B 15 20 08 E1 00 50 A1 0C F9 0A 01 68 90 B8 5C 00 52 50 E8 2F F9 72 FF 68 F0 00 00 00 68 40 01 00 00 6A 4F C7 05 FC 30 61 00 FF FF FF FF E8 04 C5 7D FF 68 F0 00 00 00 68 40 01 00 00 6A 39 E8 F3 C4 7D FF 83 C4 3C C7 05 F2 27 40 00 F6 45 0C 02 C7 05 F6 27 40 00 74 6B 39 05 E9 FA 86 6A FF 90");
            //普通护士328 资深护士364
            if (type == 328)
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
            Thread.Sleep(300);
            //300毫秒未执行，跳转主动改回
            Memo.WriteBytes(0x004027F2, "F6 45 0C 02 74 6B 39 05");

        }

        public void RightClick(int x,int y)
        {
            
            Memo.WriteBytes(0x00402E09, "90 90 90 90 90");
            Memo.WriteBytes(0x00402E1E, "90 90 90 90 90 90");
            Memo.WriteBytes(0x00402E61, "90 90 90 90 90 90");
            Memo.WriteInt(0x00CB8990, x);
            Memo.WriteInt(0x00CB8988, y);
            Memo.WriteInt(0x00CB89BC, 1);
            Thread.Sleep(100);


            //还原
            Memo.WriteBytes(0x00402E09, "A3 88 89 CB 00");
            Memo.WriteBytes(0x00402E1E, "89 0D 90 89 CB 00");
            Memo.WriteBytes(0x00402E61, "89 3D BC 89 CB 00");
            

        }

        



        

    }
}
