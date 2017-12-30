using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Pet
{
    public enum PetState
    {
        None, StandBy, Combat,Rest
    }
    public class Pet : Base

    {
        public const int BaseAddr = 0X0100A0D0;
        public const int Offset = 0x6D8;
        public const int LevelOffset = 0x8;
        public const int NameOffset = 0x6BD;
        public const int StateBase = 0x00D4C228;
        public const int StateOffset=0x4;


        public SpellList Spells { get; }

        public int Index { get; }

        public string Name => Dm.ReadString(Hwnd, (BaseAddr + NameOffset + Offset * Index).ToString("X"), 0, 20);

        public int Level => Dm.ReadInt(Hwnd, (BaseAddr + LevelOffset + Offset * Index).ToString("X"), 0);

        public PetState State =>(PetState)Dm.ReadInt(Hwnd,(StateBase+StateOffset*Index).ToString("X"),0);

        public Pet(int hwnd, int index) : base(hwnd)
        {
            Index = index;
            Spells=new SpellList(hwnd,index);
        }
    }
}
