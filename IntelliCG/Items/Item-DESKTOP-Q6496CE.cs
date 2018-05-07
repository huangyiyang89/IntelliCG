using System.Text.RegularExpressions;
using MemoLib;
namespace IntelliCG.Items
{
    public class Item : Base
    {
        //界面地址加偏移
        //第一个物品类型 00CD451C  00D219F0  血瓶43 料理23  不明26 双字节   偏移64C
        //[010AEF88]+19C+648+65C*i
        //private const string ItemTypeBase = "[010AEF88]+";
        //private const int CountBase = 0x19C + 0x644;
        //private const int TypeBase = 0x19C + 0x648;
        //private const int Offset = 0x65C;


        private const int ExistBase = 0xD213B0;
        private const int IdBase = 0xD213B4;
        private const int CountBase = 0xD213B8;
        private const int FirstLineBase = 0xD213BC;
        private const int SecondLineBase = 0xD213D9;
        private const int ThirdLineBase = 0xD21439;
        private const int FouthLineBase = 0xD21499;
        private const int FoodAndDrugTypeBase = 0xD21440;
        private const int Offset = 0x644;


        public Item(Memo memo, int index) : base(memo)
        {
            Index = index;
        }

        /// <summary>
        /// 从0开始
        /// </summary>
        public int Index { get; }

        public bool Exist => Memo.ReadInt(ExistBase + Offset * Index, 2) == 1;
        public int Id => Memo.ReadInt(IdBase + Offset * Index);
        public string Name => FirstLine;
        public string FirstLine => Memo.ReadString(FirstLineBase + Offset * Index, 30);
        public string SecondLine => Memo.ReadString(SecondLineBase + Offset * Index, 30);
        public string ThirdLine => Memo.ReadString(ThirdLineBase + Offset * Index, 30);
        public string FouthLine => Memo.ReadString(FouthLineBase + Offset * Index, 30);

        public string TypeName => Memo.ReadString(FoodAndDrugTypeBase + Offset * Index, 10);


        public bool CanBeStacked => Memo.ReadInt(CountBase + Offset * Index) != 0;

        public int Count
        {
            get
            {
                if (!CanBeStacked && Exist)
                {
                    return 1;
                }

                return Memo.ReadInt(CountBase + Offset * Index);
            }
        }

        public bool IsDrug => TypeName == "药";
        public bool IsFood => TypeName == "料理";

        /// <summary>
        /// 从8开始
        /// </summary>
        public string Position => (Index + 8).ToString("X");

        public int MaxCount
        {
            get
            {
                if (!CanBeStacked)
                {
                    return 1;
                }

                if (IsDrug || IsFood)
                {
                    return 3;
                }

                if (Name == "蕃茄酱")
                {
                    return 3;
                }

                return 20;
            }
        }

        public bool IsFull => Count == MaxCount;


        public int Index8 => Index + 8;

    }


}

