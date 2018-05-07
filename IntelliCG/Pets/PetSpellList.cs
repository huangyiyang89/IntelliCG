
using System.Collections.Generic;

using MemoLib;

namespace IntelliCG.Pets
{
    public class PetSpellList : Base
    {
        public PetSpellList(Memo memo, int petIndex) : base(memo)
        {
            PetIndex = petIndex;
            _spells = new List<PetSpell>();
            for (var i = 0; i < 10; i++)
            {
                _spells.Add(new PetSpell(memo, PetIndex, i));
            }
        }

        public int PetIndex { get; set; }

        public PetSpell this[int index] => _spells[index];
        public PetSpell this[string name] => _spells.Find(s => s.Name.Contains(name));

        private readonly List<PetSpell> _spells;


        public PetSpell EfficientSpell
        {
            get
            {
                var customNameList = new List<string>() { "连击", "乾坤", "诸刃", "崩击","冰雹魔法","火焰魔法","风刃魔法","陨石魔法"};

                foreach (var spell in _spells)
                {
                    foreach (var name in customNameList)
                    {
                        if (spell.Name.Contains(name))
                        {
                            return spell;
                        }
                    }
                }
                return null;
            }
        }

        public PetSpell StrongSpell
        {
            get
            {

                foreach (var spell in _spells)
                {

                    if (spell.Name.Contains("强力"))
                    {
                        return spell;
                    }

                }
                return null;
            }
        }

        public PetSpell HealSpell
        {
            get
            {
                var customNameList = new List<string>() { "吸血", "明镜"};
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

        public PetSpell NoManaSpell
        {
            get
            {
                var customNameList = new List<string>() { "攻击", "防御" ,"什么都不做"};
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


        public PetSpell Find(string name)
        {
            var spell = _spells.Find(s => s.Name == name);
            return spell??_spells.Find(s => s.Name.Contains(name));
        }


    }
}
