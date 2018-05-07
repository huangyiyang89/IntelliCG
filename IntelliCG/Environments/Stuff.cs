using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Environments
{
    public class Stuff:Base
    {
        

        public int Index { get;  }
        public int Base => 0x00645C70 + 0x13C * Index;
        public int NpcId=>Memo.ReadInt(Base + 0x4);
        public int XiuGe=>Memo.ReadInt(Base + 0x8);
        public bool Exist => Memo.ReadInt(Base, 1) ==2;
        public string Name  => Memo.ReadString(Base+0x54,20);
        public string PetName => Memo.ReadString(Base + 0x65,20);
        
        public int X=>(int)Memo.ReadFloat(Memo.GetPointer(Base+0x11C) + 0x274) / 64;
        public int Y=>(int)Memo.ReadFloat(Memo.GetPointer(Base+0x11C) + 0x278) / 64;
  
        public Stuff(Memo memo,int index) : base(memo)
        {
            Index = index;
        }
    }
}
