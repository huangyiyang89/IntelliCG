using System.Collections.Generic;
using System.Linq;

using MemoLib;

namespace IntelliCG.Players
{
    public class SpellList : Base
    {
        public readonly int BaseAddr = 0x00E1643C;

        public readonly int Offset = 0x49FC;
        public readonly int LevelNumber = 0x00E1643C + 0x1C;
        public readonly int IdBase = 0x00E1643C + 0x30;

        public readonly int LevelBase = 0x00E1643C + 0x3C;
        public readonly int LevelCostOffset = 0x7C;
        public readonly int LevelOffset = 0x94;
        public const int CustomIndexBase = 0x00E5BA02;
        public const int CustomIndexOffset = 0x8;



        private readonly List<Spell> _spells;

        public Spell this[int index] => _spells[index];

        public Spell this[string name] => _spells.Find(s => s.Name.Contains(name));

        public SpellList(Memo memo) : base(memo)
        {
            _spells = new List<Spell>();
            for (var i = 0; i < 11; i++)
            {
                _spells.Add(new Spell(memo, i));
            }
        }

        public Spell CustomFirstSpell => GetCustomIndexSpell(0);

        public Spell GetCustomIndexSpell(int customIndex)
        {
            var index = Memo.ReadInt(CustomIndexBase + customIndex * CustomIndexOffset, 1);
            return index < 11 ? _spells[index] : null;
        }
        public Spell EfficientSpell
        {
            get
            {
                var customNameList = new List<string>() { "乱射", "气功", "连击" };
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
            var list = new List<Spell>();
            for (var i = 0; i < 11; i++)
            {
                var spell = GetCustomIndexSpell(i);
                if (spell != null)
                {
                    list.Add(spell);
                }
            }

            return list;
        }


        
        //返回自定义位置上第一个符合的
        public Spell FindTheFirst(params string[] names)
        {
            var list = GetSpellsOrderByCustomIndex();
            foreach (var spell in list)
            {
                if (names.Any(name => spell.Name.Contains(name)))
                {
                    return spell;
                }
            }
            return null;
        }

        /// <summary>
        /// 能够查找名字一样的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Spell FindExact(string name)
        {
            var spells = GetSpellsOrderByCustomIndex();
            var spell = spells.Find(s => s.Name == name);
            return spell ?? spells.Find(s => s.Name.Contains(name));
        }




    }
}
