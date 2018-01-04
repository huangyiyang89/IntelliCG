using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;

namespace IntelliCG.Pet
{
    public enum PetState
    {
        None, StandBy, Combat,Rest
    }
    public class Pet : Base

    {
        internal const int BaseAddr = 0X0100A0D0;
        internal const int Offset = 0x6D8;
        internal const int LevelOffset = 0x8;
        internal const int NameOffset = 0x6BD;
        internal const int StateOffset = 0x6AA;
        


        public SpellList Spells { get; }

        public int Index { get; }

        public string Name => Memo.ReadString(BaseAddr + NameOffset + Offset * Index, 20);

        public int Level => Memo.ReadInt(BaseAddr + LevelOffset + Offset * Index);

        public PetState State => (PetState) Memo.ReadInt(BaseAddr + StateOffset + Offset * Index, 1);

        public Pet(Memo memo, int index) : base(memo)
        {
            Index = index;
            Spells=new SpellList(memo,index);
        }
    }
}
