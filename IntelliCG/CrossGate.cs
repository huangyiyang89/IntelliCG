using System.Threading;
using System.Threading.Tasks;
using IntelliCG.Item;
using IntelliCG.Pet;
using SpellList = IntelliCG.Player.SpellList;

namespace IntelliCG
{
    public class CrossGate:Base
    {
        
        public Player.Player Player { get;}
        public Cheat.Cheat Cheat { get; }

        public ItemList Items { get; }
        public Window Window { get; }
        public Combat.Combat Combat { get;}
        
        public PetList Pets { get; }


        public CrossGate(int hwnd):base(hwnd)
        {
            Cheat = new Cheat.Cheat(hwnd);
            Player=new Player.Player(hwnd);
            Combat=new Combat.Combat(hwnd);
            Items=new ItemList(hwnd);
            Window=new Window(hwnd);
            Pets=new PetList(hwnd);
        }

        

        public bool IsExist => Dm.GetWindowState(Hwnd, 0) == 1;

        public void MoveTo(int x, int y)
        {
            Dm.WriteData(Hwnd, "0048940D", "B8 00 00 00 00");
            Dm.WriteData(Hwnd, "00489431", "BA 00 00 00 00 90");
            Dm.WriteInt(Hwnd, "0048940E", 0, x);
            Dm.WriteInt(Hwnd, "00489432", 0, y);
            Dm.WriteInt(Hwnd, "00CB89B0", 0, 1);
            Thread.Sleep(100);
            Dm.WriteInt(Hwnd, "00CB89B0", 0, 0);
            Dm.WriteData(Hwnd, "0048940D", "A1 90 89 CB 00");
            Dm.WriteData(Hwnd, "00489431", "8B 15 88 89 CB 00");
        }

        public void Call(string content)
        {
            //call地址 00518B75
            //00D57F68
            Dm.WriteData(Hwnd, "00D5A000", content);
            var len = content.Split(' ').Length;
            Dm.AsmClear();
            Dm.AsmAdd("push 00");
            Dm.AsmAdd("push " + "0" + len.ToString("X"));
            Dm.AsmAdd("push 00D5A000");
            Dm.AsmAdd("push [00E10820]");
            Dm.AsmAdd("call 00564BDE");
            Dm.AsmCall(Hwnd, 1);
        }


        public void Attack(string position)
        {
            //空地址，作为人物命令缓存
            Dm.WriteString(Hwnd, "00D59000", 0, "S|2|0|A");
            //空地址，作为宠物命令缓存
            Dm.WriteString(Hwnd, "00D59010", 0, "W|1|1");
            //判断是否选到了怪
            Dm.WriteData(Hwnd, "004CD5E4", "B8 05 00 00 00");
            //判断鼠标状态 点到什么
            Dm.WriteInt(Hwnd, "006455D8", 0, 0);
            //是否点击了的判断
            Dm.WriteInt(Hwnd, "00D57EE4", 0, 1);


            //改人物命令字符串缓存地址
            Dm.WriteData(Hwnd, "004CD625", "68 00 90 D5 00 90 90");
            //改宠物命令字符串缓存地址
            Dm.WriteData(Hwnd, "004CDA0E", "68 10 90 D5 00 90 90");

            //_dm.WriteInt(Hwnd, "005EE830", 0, 3);判断是否选择了宠物技能
            //判断是否选择了宠物技能
            Dm.WriteData(Hwnd, "004CD9E7", "90 90");
        }


        

        
        //00E150B0   00FF53F0
        



        

    }
}
