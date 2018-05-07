
using MemoLib;

namespace IntelliCG.Pets
{
    public class PetSpell : Base
    {

        internal const int BaseAddr = Pet.BaseAddr + 0xD8;
        internal const int Offset = 0x8C;
        internal const int NameOffset = 0x8;
        internal const int MpCostOffset = 0x84;

        public int PetIndex { get;}
        public int Index { get;}
        public string Name => Memo.ReadString(BaseAddr + Pet.Offset * PetIndex + Offset * Index + NameOffset,20);
        public int MpCost => Memo.ReadInt(BaseAddr + Pet.Offset * PetIndex + Offset * Index + MpCostOffset);

        public PetSpell(Memo memo,int petIndex,int index) : base(memo)
        {
            PetIndex = petIndex;
            Index = index;
        }
    }
}
