using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Pet
{
    public class Spell : Base
    {

        public const int BaseAddr = Pet.BaseAddr + 0xD8;
        public const int Offset = 0x8C;
        public const int NameOffset = 0x8;
        public const int MpCostOffset = 0x84;

        public int PetIndex { get;}
        public int Index { get;}
        public string Name => Dm.ReadString(Hwnd, (BaseAddr+Pet.Offset*PetIndex+Offset*Index+NameOffset).ToString("X"), 0, 20);
        public int MpCost => Dm.ReadInt(Hwnd, (BaseAddr+Pet.Offset* PetIndex+Offset* Index+MpCostOffset).ToString("X"), 0);

        public Spell(int hwnd,int petIndex,int index) : base(hwnd)
        {
            PetIndex = petIndex;
            Index = index;
        }
    }
}
