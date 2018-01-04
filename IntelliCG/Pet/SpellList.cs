using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;

namespace IntelliCG.Pet
{
    public class SpellList:Base
    {
        public SpellList(Memo memo,int petIndex) : base(memo)
        {
            PetIndex = petIndex;
            _spells=new List<Spell>();
            for (var i = 0; i < 10; i++)
            {
                _spells.Add(new Spell(memo,PetIndex,i));
            }
        }

        public int PetIndex { get; set; }

        public Spell this[int index] => _spells[index];
        public Spell this[string name] => _spells.Find(s=>s.Name.Contains(name));

        private readonly List<Spell> _spells;

        public Spell EfficientSpell
        {
            get
            {
                var customNameList = new List<string>() { "连击", "乾坤", "陨石魔法", "冰雹魔法", "风刃魔法", "火焰魔法", "攻击", "防御" };
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


        public Spell HealSpell
        {
            get
            {
                var customNameList = new List<string>() { "吸血", "明镜", "攻击", "防御" };
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

        public Spell NoManaSpell
        {
            get
            {
                var customNameList = new List<string>() { "攻击", "防御" };
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
        


    }
}
