using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;

namespace IntelliCG.Player
{
    public class SpellList:Base
    {
        public readonly int BaseAddr = 0x00E1643C;

        public readonly int Offset = 0x49FC;
        public readonly int LevelNumber = 0x00E1643C+0x1C;
        public readonly int IdBase= 0x00E1643C+0x30;
        
        public readonly int LevelBase = 0x00E1643C+0x3C;
        public readonly int LevelCostOffset = 0x7C;
        public readonly int LevelOffset = 0x94;
        public const int CustomIndexBase= 0x00E5BA02;
        public const int CustomIndexOffset = 0x8;



        private readonly List<Spell> _spells;

        public Spell this[int index] => _spells[index];

        public Spell this[string name]=> _spells.First(s => s.Name.Contains(name));
        

        public SpellList(Memo memo) : base(memo)
        {
            _spells = new List<Spell>();
            for (var i = 0; i < 10; i++)
            {
                _spells.Add(new Spell(memo,i));
            }
        }

        public Spell CustomFirstSpell => GetCustomIndexSpell(0);

        public Spell GetCustomIndexSpell(int customIndex)
        {
            var index = Memo.ReadInt(CustomIndexBase + customIndex * CustomIndexOffset, 1);
            return _spells[index];
        }
        public Spell EfficientSpell
        {
            get
            {
                var customNameList = new List<string>() { "乱射", "气功", "连击"};
                foreach (var customName in customNameList)
                {
                    var found = _spells.Find(s => s.Name.Contains(customName));
                    if (found != null)
                    {
                        return found;
                    }
                }
                return null;
            }
        }

        public List<Spell> GetSpellsOrderByCustomIndex()
        {
            var list=new List<Spell>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(GetCustomIndexSpell(i));
            }

            return list;
        }

        //返回自定义位置上第一个
        public Spell FindEqualsFirst(params string[] names)
        {
            return GetSpellsOrderByCustomIndex().Find(s => names.ToList().Contains(s.Name));
        }

       



        

    }
}
