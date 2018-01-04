using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;
using IntelliCG.Player;

namespace IntelliCG.Combat
{


    public class Combat : Base
    {


        public UnitList Units { get; }

        public Combat(Memo memo) : base(memo)
        {
            Units = new UnitList(memo);
        }

        //战斗中3 战斗结束画面1 非战斗0
        public bool IsFighting => Memo.ReadInt(0x01009530, 2) > 0;
        //人物行动1 宠物行动4 结束战斗5
        public bool IsPlayerTurn => Memo.ReadInt(0x006456C0) == 1;
        public bool IsPlayerFirstTurn => !IsPlayerSecondTurn && IsPlayerTurn;
        public bool IsPlayerSecondTurn => IsPlayerTurn && Memo.ReadInt(0x00645674) == 1;
        public bool IsPetTurn => Memo.ReadInt(0x006456C0) == 4;
        public bool HasUsedSpell => Memo.ReadInt(0x00645634) == 1;
        public int Round => Memo.ReadInt(0x00645698);

        private void PlayerAction(string playerAction)
        {
            Memo.WriteBytes(0x004CD625, "68-00-90-D5-00-90-90");
            Memo.WriteString(0x00D59000, playerAction + " ");
            Memo.WriteInt(0x006455D8, 0);
            Memo.WriteBytes(0x004CD5EC, "90-90-90-90-90-90");
            Memo.WriteInt(0x00D57EE4, 1);
            Thread.Sleep(100);
            Memo.WriteBytes(0x004CD625, "8D-95-FC-FE-FF-FF-52");
            Memo.WriteBytes(0x004CD5EC, "0F-84-27-01-00-00");
        }


        private void PetAction(string petAction)
        {
            //改宠物命令字符串缓存地址
            Memo.WriteBytes(0x004CDA0E, "68-10-90-D5-00-90-90");
            Memo.WriteString(0x00D59010, petAction + " ");
            Memo.WriteInt(0x005EE830, 3);
            Memo.WriteBytes(0x004CD9DF, "90-90");
            Memo.WriteInt(0x00D57EE4, 1);
            Thread.Sleep(100);
            Memo.WriteBytes(0x004CDA0E, "8D-85-FC-FE-FF-FF-50");
            Memo.WriteBytes(0x004CD9DF, "74-5F");
        }
        

        public void Cast(Unit target)
        {
            PlayerAction("H|" + target.Index.ToString("X"));
        }
        public void Cast(Item.Item item, Unit target)
        {
            PlayerAction("I|" + item.Position + "|" + target.Position);
        }

        public void Cast(Spell spell, Unit target, int level = 9)
        {
            var useLevel = level > 9 ? 9 : level;
            useLevel = spell.Level < useLevel ? spell.Level : useLevel;

            var position = target.Index;

            if (target.IsEnemy)
            {
                if (spell.Name.Contains("超强"))
                {
                    position = 0x29;
                }

                if (spell.Name.Contains("强力"))
                {
                    position = position + 20;
                }
            }
            else
            {
                if (spell.Name.Contains("超强"))
                {
                    position = 0x28;
                }

                if (spell.Name.Contains("强力"))
                {
                    position = position + 20;
                }
            }




            PlayerAction("S|" + spell.Index.ToString("X") + "|" + useLevel.ToString("X") + "|" + position.ToString("X"));
        }
        public void PetCast(Pet.Spell spell, Unit target)
        {
            PetAction("W|" + spell.Index.ToString("X") + "|" + target.Index.ToString("X"));
        }
        

        







    }
}
