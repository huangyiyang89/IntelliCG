using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Item
{
    public class Item:Base
    {
        //第一个物品类型 00CD451C  00D219F0  血瓶43 料理23  不明26 双字节   偏移64C
        //[010AEF88]+19C+648+65C*i

        //private const string ItemTypeBase = "[010AEF88]+";
        //private const int CountBase = 0x19C + 0x644;
        //private const int TypeBase = 0x19C + 0x648;
        //private const int Offset = 0x65C;

        public const int ExistBase = 0xD213B0;
        public const int IdBase = 0xD213B4;
        public const int CountBase = 0xD213B8;
        public const int FirstLineBase = 0xD213BC;
        public const int SecondLineBase = 0xD213D9;
        public const int ThirdLineBase = 0xD21439;
        public const int FouthLineBase = 0xD21499;

        public const int FoodAndDrugTypeBase = 0xD21440;
        private const int Offset = 0x644;


        public Item(int hwnd,int index) : base(hwnd)
        {
            Index = index;
        }

        /// <summary>
        /// 0开始
        /// </summary>
        public int Index { get; }
        public bool Exist => Dm.ReadInt(Hwnd, (ExistBase + Offset * Index).ToString("X"), 0)==1;
        public int Id => Dm.ReadInt(Hwnd, (IdBase + Offset * Index).ToString("X"), 0);
        public string FirstLine => Dm.ReadString(Hwnd, (FirstLineBase+Offset* Index).ToString("X"), 0, 30);
        public string SecondLine => Dm.ReadString(Hwnd, (SecondLineBase + Offset * Index).ToString("X"), 0, 30);
        public string ThirdLine => Dm.ReadString(Hwnd, (ThirdLineBase + Offset * Index).ToString("X"), 0, 30);
        public string FouthLine => Dm.ReadString(Hwnd, (FouthLineBase + Offset * Index).ToString("X"), 0, 30);

        public string Type => Dm.ReadString(Hwnd, (FoodAndDrugTypeBase + Offset * Index).ToString("X"), 0, 10);

        /// <summary>
        /// 8开始
        /// </summary>
        public string Position => Index.ToString("X");

       
    }
}
