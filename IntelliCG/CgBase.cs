using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG
{
    public class CgBase:Base
    {
        public CgBase(CrossGate cg) : base(cg.Memo)
        {
            Cg = cg;
        }

        public CrossGate Cg { get; set; }
    }
}
