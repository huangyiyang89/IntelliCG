using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;

namespace IntelliCG.Player
{
    

    public class Player:Base
    {
        //[010AEF88]+19C+648+65C*3    4,5,6 弓4 回力5 小刀6
        //[010AEF88]+19C+648+65C*3  


        public SpellList Spells { get; }

        public Player(Memo memo) : base(memo)
        {
            
            Spells=new SpellList(memo);
        }

        public string Name => Memo.ReadString(0x00E150B0, 14);

        public bool IsMoving => Memo.ReadInt(0x005F6A48) != 65535;

        public Job Job
        {
            get
            {
                var jobName = Memo.ReadString(0x00E16420, 20);
                if (jobName.Contains("弓")) return Job.Gongshou;
                if (jobName.Contains("格")) return Job.Gedou;
                if (jobName.Contains("魔")) return Job.Mofashi;
                if (jobName.Contains("驯兽")) return Job.Xunshou;
                if (jobName.Contains("教") || jobName.Contains("牧师")) return Job.Chuanjiao;
                if (jobName.Contains("巫")) return Job.Wushi;
                if (jobName.Contains("咒")|| jobName.Contains("降头师")) return Job.Zhoushu;
                return Job.Others;
            }
        }

        public Weapon Weapon
        {
            get
            {
                var leftWeapon = Memo.ReadInt(Memo.GetPointer(0x010AEF88) + 0x19C + 0x648 + 0x65C * 2);
                var rightWeapon= Memo.ReadInt(Memo.GetPointer(0x010AEF88) + 0x19C + 0x648 + 0x65C * 3);
                if (leftWeapon == 4|| rightWeapon==4)
                {
                    return Weapon.Gong;
                }
                if (leftWeapon == 5 || rightWeapon == 5)
                {
                    return Weapon.HuiLi;
                }
                if (leftWeapon == 6 || rightWeapon == 6)
                {
                    return Weapon.XiaoDao;
                }
                return Weapon.Others;
            }
        } 
        
        public int X => Convert.ToInt32(Memo.ReadFloat(0x00C1EF10) / 64);
        public int Y => Convert.ToInt32(Memo.ReadFloat(0x00C20384) / 64);

        public int Hp => Convert.ToInt32(Memo.ReadString(0x00CF8AC8, 10).Split('/')[0]);
        public int HpMax => Convert.ToInt32(Memo.ReadString(0x00CF8AC8, 10).Split('/')[1]);
        public int Mp => Convert.ToInt32(Memo.ReadString(0x00D56488, 10).Split('/')[0]);
        public int MpMax => Convert.ToInt32(Memo.ReadString(0x00D56488, 10).Split('/')[1]);

        public int HpPer => Hp * 100 / HpMax;

        public int MpPer => Mp * 100 / MpMax;

        public int MapId => Memo.ReadInt(0x00C0617C);
    }
}
