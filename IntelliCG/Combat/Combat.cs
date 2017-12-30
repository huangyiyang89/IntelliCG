using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dm;
using IntelliCG.Player;

namespace IntelliCG.Combat
{


    public class Combat : Base
    {
       
        
        public UnitList Units { get; }

        public Combat(int hwnd) : base(hwnd)
        {
            Units = new UnitList(hwnd);
        }

        public bool IsFighting => Dm.ReadInt(Hwnd, "0100A0E8", 0) > 0;
        public bool IsPlayerTurn => Dm.ReadInt(Hwnd, "006456C0", 0) == 1;
        public bool IsPlayerSecondTurn => IsPlayerTurn && Dm.ReadInt(Hwnd, "00645674", 0) == 1;
        public bool IsPetTurn => Dm.ReadInt(Hwnd, "006456C0", 0) == 4;
        public bool HasUsedSpell => Dm.ReadInt(Hwnd, "00645634", 0) == 1;
        public int Round => Dm.ReadInt(Hwnd, "00645698", 0);
        
        public void PlayerAction(string playerAction)
        {
            //改人物命令字符串缓存地址
            Dm.WriteData(Hwnd, "004CD625", "68 00 90 D5 00 90 90");
            //空地址，作为人物命令缓存
            Dm.WriteString(Hwnd, "00D59000", 0, playerAction);
            //判断选择了技能还是普攻  普攻0 技能2
            Dm.WriteInt(Hwnd, "006455D8", 0, 0);
            //判断鼠标滑到了怪
            Dm.WriteData(Hwnd, "004CD5EC", "90 90 90 90 90 90");
            //是否点击了的判断
            Dm.WriteInt(Hwnd, "00D57EE4", 0, 1);
            Thread.Sleep(100);
            //改回人物命令字符串缓存地址
            Dm.WriteData(Hwnd, "004CD625", "8D 95 FC FE FF FF 52");
            //改回判断鼠标滑到了怪
            Dm.WriteData(Hwnd, "004CD5EC", "0F 84 27 01 00 00");
        }


        public void PetAction(string petAction)
        {
            //改宠物命令字符串缓存地址
            Dm.WriteData(Hwnd, "004CDA0E", "68 10 90 D5 00 90 90");
            //空地址，作为宠物命令缓存
            Dm.WriteString(Hwnd, "00D59010", 0, petAction);

            //判断是否选择了宠物技能
            Dm.WriteInt(Hwnd, "005EE830", 0, 3);
            //宠物判断鼠标滑到了怪
            Dm.WriteData(Hwnd, "004CD9DF", "90 90");
            //是否点击了的判断
            Dm.WriteInt(Hwnd, "00D57EE4", 0, 1);
            Thread.Sleep(100);
            //改回宠物命令字符串缓存地址
            Dm.WriteData(Hwnd, "004CDA0E", "8D 85 FC FE FF FF 50");
            //宠物判断鼠标滑到了怪
            Dm.WriteData(Hwnd, "004CD9DF", "74 5F");
        }

        

        public void Cast(Unit target)
        {
            PlayerAction("H|" + target.Index.ToString("X"));
        }

        public void Cast(Spell spell, Unit target, int level = 9)
        {
            var useLevel=level>9?9:level;
            useLevel = spell.Level < useLevel ? spell.Level : useLevel;
            PlayerAction("S|" + spell.Index.ToString("X") + "|" + useLevel.ToString("X") + "|" + target.Index.ToString("X"));
        }
        public void PetCast(Pet.Spell spell, Unit target)
        {
            PetAction("W|" + spell.Index.ToString("X") + "|" + target.Index.ToString("X"));
        }

        public void PetCast(Pet.Spell spell)
        {
            PetAction("W|" + spell.Index.ToString("X"));
        }

        public void UseItem(Item.Item item, Unit target)
        {
           PlayerAction("I|"+item.Position+"|"+target.Position);
        }

       
       




    }
}
