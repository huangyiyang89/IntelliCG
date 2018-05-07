using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Environments
{
    public class StuffList : Base
    {

        private readonly List<Stuff> _stuffs;
        public StuffList(Memo memo) : base(memo)
        {
            _stuffs = new List<Stuff>();
            for (var i = 0; i < 50; i++)
            {
                var stuff = new Stuff(memo, i);
                _stuffs.Add(stuff);
            }

        }


        public List<Stuff> ReadRange(int x,int y, int range)
        {
            var list = new List<Stuff>();
            foreach (var stuff in _stuffs)
            {
                if (stuff.Exist&&stuff.PetName.Contains("-")&&Math.Abs(stuff.X - x) <= range &&Math.Abs(stuff.Y - y) <= range)
                {
                    list.Add(stuff);
                }
            }
            return list;
        }

        public Stuff FindXiuGe()
        {
            return _stuffs.FirstOrDefault(s => s.Name == "修葛"&&s.XiuGe>0);
        }
        
    }
}
