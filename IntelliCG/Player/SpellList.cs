using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

       

        private readonly List<Spell> _spells;

        public Spell this[int index] => _spells[index];

        public Spell this[string name]=> _spells.First(s => s.Name.Contains(name));
        

        public SpellList(int hwnd) : base(hwnd)
        {
            _spells = new List<Spell>();
            for (var i = 0; i < 12; i++)
            {
                _spells.Add(new Spell(Hwnd,i));
            }
        }

        public Spell CustomFirstSpell
        {
            get
            {
                const int baseAddr = 0x00E5BA02;
                var index = Dm.ReadInt(Hwnd, (baseAddr + 0 * 8).ToString("X"), 2);
                return _spells[index];
            }
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

       



        

    }
}
