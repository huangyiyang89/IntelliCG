using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Produces
{
    public class Contact : Base
    {
        public int Base=> 0x00EBA152 + Index * 0x74;
        
        public int Line=> Memo.ReadInt(Base, 1);
        public string Name => Memo.ReadString(Base+0xE, 20);

        public bool Online => Line > 0;

        public int Index { get; }

        public Contact(Memo memo,int index) : base(memo)
        {
            Index = index;
        }
        
    }
}
