using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Player
{

    //public enum SpellType
    //{
    //    Attack,Heal,Others
    //}
    //public enum SpellTargetType {
    //Single,Cross,All,Others
    //}

    public class Spell : Base
    {
        //public readonly int BaseAddr = 0x00E1643C;

        //public readonly int Offset = 0x49FC;
        //public readonly int LevelNumber = 0x00E1643C + 0x1C;
        //public readonly int IdBase = 0x00E1643C + 0x30;

        //public readonly int LevelBase = 0x00E1643C + 0x3C;

        //public readonly int LevelOffset = 0x94;
        /// <summary>
        /// Name Base
        /// </summary>
        
        public const int NameBase = 0xE1643C;
        public const int IdBase = NameBase + 0x30;
        public const int LevelNumberBase = NameBase + 0x1C;
        public const int Offset = 0x49FC;

        public const int LevelNameBase= NameBase+0x3C;
        public const int LevelCostBase =NameBase + 0xB8;
        public const int LevelOffset = 0x94;


        public Spell(int hwnd, int index) : base(hwnd)
        {
            Index = index;
        }

        public int Index { get; }

        public bool Exist => Name != "";

        public string Name => Dm.ReadString(Hwnd, (NameBase + Offset * Index).ToString("X"), 0, 20);


        public int Level => Dm.ReadInt(Hwnd, (LevelNumberBase + Offset * Index).ToString("X"), 0)-1;

        //public SpellType SkillType
        //{
        //    get
        //    {
        //        if (Name.Contains("补血") 
        //            || Name.Contains("恢复")
        //            )
        //        {
        //            return SpellType.Heal;
        //        }
        //        if (Name.Contains("") 
        //            || Name.Contains("")
        //        )
        //        {
        //            return SpellType.Attack;
        //        }
        //        return SpellType.Others;

        //    }
        //}

        //public SpellTargetType SkillSpellType
        //{
        //    get
        //    {
        //        if (Name.Contains("强力"))
        //        {
        //            return SpellTargetType.Cross;
        //        }
        //        if (Name.Contains("超强")
        //        )
        //        {
        //            return SpellTargetType.All;
        //        }

        //        return SpellTargetType.Others;
        //    }
        //}

        public int MpCost => Dm.ReadInt(Hwnd,(LevelCostBase + Offset * Index + LevelOffset * Level).ToString("X"), 0);

        public bool Is(string name)
        {
            return Name.Contains(name);
        }
    }
}

