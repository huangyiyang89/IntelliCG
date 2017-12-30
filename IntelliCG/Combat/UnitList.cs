using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Combat
{
    public class UnitList : Base
    {
        private readonly Random _random;
        public List<Unit> LastRoundUnits { get; set; }
        private readonly List<Unit>  _units;
        public List<Unit> Enemys { get; }
        public List<Unit> Friends { get; }

        public UnitList(int hwnd) : base(hwnd)
        {
            //_lastRound = 0;
            //_lastTime = 0;
            _units = new List<Unit>();
            LastRoundUnits = new List<Unit>();
            Enemys = new List<Unit>();
            Friends = new List<Unit>();
            _random=new Random(_units.GetHashCode());
        }

        public Unit this[int index]
        {
            get { return _units.Find(u => u.Index == index); }
        }
        public Unit this[string position]
        {
            get { return _units.Find(u => u.Position == position); }
        }
        //private int Round => Dm.ReadInt(Hwnd, "00645698", 0);


        //private int _lastRound;
        //private long _lastTime;
        public UnitList Read()
        {
            //if (_lastRound == Round && DateTime.Now.Ticks - _lastTime < 2000 * 10000)
            //{
            //    return this;
            //}
            //_lastRound = Round;
            //_lastTime = DateTime.Now.Ticks;
            LastRoundUnits = _units;
            _units.Clear();
            Enemys.Clear();
            Friends.Clear();

            var roundMod = Dm.ReadInt(Hwnd, "00645378", 2) + 1;
            var content = Dm.ReadString(Hwnd, "0064" + roundMod + "230", 0, 2000);
            if (content.Length <= 20)
            {
                return this;
            }

            var entryList = content.Substring(4).Split('|').ToList();

            for (var i = 0; i < entryList.Count-1; i+=12)
            {
                var unit = new Unit(entryList.GetRange(i, 11), this);
                _units.Add(unit);
                if (unit.IsEnemy) { Enemys.Add(unit); }
                else
                {
                    Friends.Add(unit);
                }
            }
            
            
            return this;
        }

        public Unit Player => this[Dm.ReadInt(Hwnd, "645710", 0)];

        public Unit Pet
        {
            get
            {
                var index = Player.Index > 4 ? Player.Index - 5 : Player.Index + 5;
                return this[index];
            }
        }

        public Unit GetRandomEnemy()
        {
            return Enemys[_random.Next(Enemys.Count-1)];
        }
        public Unit GetRandomEnemy(bool mustFront)
        {
            if (!mustFront)
            {
                return GetRandomEnemy();
            }
            var result = Enemys.FindAll(e => e.FrontUnit == null);
            return this[result[_random.Next(result.Count-1)].Index];
        }
        public Unit GetRandomFrontEnemy()
        {
            var result=_units.Where(u => u.IsEnemy && u.FrontUnit == null).ToList();
            return this[result[_random.Next(result.Count-1)].Index];
        }

    }
}
