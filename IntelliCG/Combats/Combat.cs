using System;
using System.Threading.Tasks;
using IntelliCG.Combats;
using IntelliCG.Extensions;
using IntelliCG.Players;
using IntelliCG.Pets;
using MemoLib;

namespace IntelliCG.Combats
{
    
    public class Combat : CgBase
    {


        public UnitList Units { get; }

        public Combat(CrossGate cg) : base(cg)
        {
            Units = new UnitList(cg.Memo);
        }

        public bool IsOvering=>Memo.ReadInt(0x01009530, 2) ==1;
        //战斗中3 战斗结束画面1 非战斗0
        public bool IsFighting => Memo.ReadInt(0x01009530, 2) > 0;
        //人物行动1 宠物行动4 结束战斗5
        public bool IsPlayerTurn => Memo.ReadInt(0x006456C0) == 1;
        public bool IsPlayerFirstTurn => !IsPlayerSecondTurn && IsPlayerTurn;
        public bool IsPlayerSecondTurn => IsPlayerTurn && Memo.ReadInt(0x00645674) == 1;
        public bool IsPetTurn => Memo.ReadInt(0x006456C0) == 4;
        public bool HasUsedSpell => Memo.ReadInt(0x00645634) == 1;
        public int Round => Memo.ReadInt(0x00645698);

        private async Task PlayerAction(string playerAction)
        {
            Memo.WriteBytes(0x004CD625, "68-00-90-D5-00-90-90");
            Memo.WriteString(0x00D59000, playerAction + " ");
            Memo.WriteInt(0x006455D8, 0);
            Memo.WriteBytes(0x004CD5EC, "90-90-90-90-90-90");
            Memo.WriteBytes(0x004CD5F8, "90-90-90-90-90-90");
            //Memo.WriteInt(0x00D57EE4, 1);
            await Task.Delay(100);//延时低可能点不上
            Memo.WriteBytes(0x004CD625, "8D-95-FC-FE-FF-FF-52");
            Memo.WriteBytes(0x004CD5EC, "0F-84-27-01-00-00");
            Memo.WriteBytes(0x004CD5F8, "0F-84-1B-01-00-00");
            Console.WriteLine(playerAction);
            await Task.Delay(100);
        }

        private async Task PetAction(string petAction)
        {
            //改宠物命令字符串缓存地址
            Memo.WriteBytes(0x004CDA0E, "68-10-90-D5-00-90-90");
            Memo.WriteString(0x00D59010, petAction + " ");
            Memo.WriteInt(0x005EE830, 3);
            Memo.WriteBytes(0x004CD9DF, "90-90");
            Memo.WriteBytes(0x004CD9E7, "90-90");
            //Memo.WriteInt(0x00D57EE4, 1);
            await Task.Delay(100);//延时低可能点不上
            Memo.WriteBytes(0x004CDA0E, "8D-85-FC-FE-FF-FF-50");
            Memo.WriteBytes(0x004CD9DF, "74-5F");
            Memo.WriteBytes(0x004CD9E7, "74-57");
            Console.WriteLine(petAction);
            await Task.Delay(100);
        }

        public async Task Escape()
        {
            Memo.WriteBytes(0x0041C248, "90 90");
            Memo.WriteBytes(0x0041C285, "90 90 90 90 90 90");
            await Task.Delay(100);
            Memo.WriteBytes(0x0041C248, "74 38");
            Memo.WriteBytes(0x0041C285, "0F 84 9A 00 00 00");
            await Task.Delay(500);
        }

        public async Task Cast(Unit target)
        {
            await PlayerAction("H|" + target.Index.ToString("X"));
        }

        public async Task Cast(Items.Item item, Unit target)
        {
            await PlayerAction("I|" + item.Index8.ToString("X") + "|" + target.Position);
        }

        public async Task Cast(Spell spell, Unit target, int level = 9)
        {

            var useLevel = level > 9 ? 9 : level;
            useLevel = useLevel < 0 ? 0 : useLevel;
            useLevel = spell.Level < useLevel ? spell.Level : useLevel;
            var canUse = Cg.Player.Level / 10;
            useLevel = useLevel > canUse ? canUse : useLevel;

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
                if (spell.Name.Contains("洁净") && useLevel > 2)
                {
                    position = position + 20;
                }
                if (spell.Name.Contains("洁净") && useLevel > 5)
                {
                    position = 0x28;
                }
            }




            await PlayerAction("S|" + spell.Index.ToString("X") + "|" + useLevel.ToString("X") + "|" + position.ToString("X"));
        }

        public async Task Cast(Spell spell, int level = 9)
        {
            var useLevel = level > 9 ? 9 : level;
            useLevel = spell.Level < useLevel ? spell.Level : useLevel;



            await PlayerAction("S|" + spell.Index.ToString("X") + "|" + useLevel.ToString("X")+"|0");
        }

        public async Task PetCast(PetSpell spell, Unit target)
        {
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

            await PetAction("W|" + spell.Index.ToString("X") + "|" + position.ToString("X"));
        }

        public async Task PetCast(PetSpell spell)
        {
            await PetAction("W|" + spell.Index.ToString("X"));
        }

        //public event EventHandler CombatStart;
        //public event EventHandler CombatOver;
        //public event EventHandler PlayerFirstTurn;
        //public event EventHandler PlayerTurn;
        //public event EventHandler Turn;
        //private bool _combatFlag;
        
        //public void GetCombatState()
        //{
        //    if (!IsFighting)
        //    {
        //        //结束战斗
        //        if (!_combatFlag) return;
        //        _combatFlag = false;
        //        CombatOver?.Invoke(this,null);
        //        return;
        //    }
            
        //    if (_combatFlag == false)
        //    {
        //        //进入战斗
        //        _combatFlag = true;
        //        CombatStart?.Invoke(this,null);
        //    }

        //    if (IsPlayerFirstTurn)
        //    {
        //        Units.Read();
        //        PlayerFirstTurn?.Invoke(this,null);
        //    }

        //    if (IsPlayerTurn)
        //    {
        //        PlayerTurn?.Invoke(this,null);
        //    }

        //    if (IsPlayerTurn || IsPetTurn)
        //    {
        //        Turn?.Invoke(this,null);
        //    }

        //    //await RunAwayExit();
        //}


        public async Task RunAwayExit()
        {
            //if (Memo.ReadInt(0x006456C0) == 5&&Memo.ReadInt(0x00645674) == 1&&Memo.ReadInt(0x01009530, 2)==3)
            //{
            if (Memo.ReadBytes(0x006454B8, 20) == "01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01")
            {
                await Task.Delay(500);
                if (Memo.ReadBytes(0x006454B8, 20) == "01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01-01")
                {
                    Memo.WriteInt(0x00645554, 1);
                    Memo.WriteBytes(0x006454B8, "01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00");
                    await Task.Delay(500);
                }
            }
        }


        public async Task Encounter()
        {
            var str = $@"Y21 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} ";
            await Cg.Actions.DecodeSend(str);
            await Task.Delay(1500);
        }

    }
}
