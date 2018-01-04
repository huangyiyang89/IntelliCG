using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;
using IntelliCG.Player;

namespace IntelliCG.Combat
{
    public class UnitList : Base
    {
        private readonly Random _random;
        public List<Unit> LastRoundUnits { get; set; }
        private readonly List<Unit>  _units;
        public List<Unit> Enemys { get; }
        public List<Unit> Friends { get; }

        public UnitList(Memo memo) : base(memo)
        {
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
        public UnitList Read()
        {
            LastRoundUnits = _units;
            _units.Clear();
            Enemys.Clear();
            Friends.Clear();

            var roundMod = Memo.ReadInt(0x00645378, 1) + 1;
            var content = Memo.ReadString(0x00640230 + roundMod * 0x1000, 2000);
            if (content.Length <= 20)
            {
                return this;
            }

            var entryList = content.Substring(4).Split('|').ToList();

            for (var i = 0; i < entryList.Count-1; i+=12)
            {
                var unit = new Unit(entryList.GetRange(i, 11), this);
                _units.Add(unit);
                Console.WriteLine($@"{unit.Index},{unit.Name}");
                if (unit.IsEnemy) { Enemys.Add(unit); }
                else
                {
                    Friends.Add(unit);
                }
            }
            return this;
        }

        public Unit Player => this[Memo.ReadInt(0x645710)];//人物位置

        public Unit Pet
        {
            get
            {
                var index = Player.Index > 4 ? Player.Index - 5 : Player.Index + 5;
                return this[index];
            }
        }

        public List<Unit> FindAll(params int[] indexs)
        {
            return _units.FindAll(u => indexs.Contains(u.Index));
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

        

        public Unit GetTargetByWeapon(Weapon weapon)
        {
            if (weapon == Weapon.HuiLi)
            {
                var front=Enemys.FindAll(e => e.IsInFront).Count;
                var back = Enemys.Count - front;
                return front > back ? Enemys.Find(e => e.IsInFront) : Enemys.Find(e => !e.IsInFront);
            }

            if (weapon == Weapon.Gong)
            {
                GetRandomEnemy();

            }

            if (weapon == Weapon.XiaoDao)
            {
                var target=Enemys.Find(e => e.FrontUnit != null);
                return target ?? GetRandomEnemy();
            }
            
            return GetRandomFrontEnemy();
            
        }

        public bool HasLowHpFriend()
        {
            var friend = Friends.Find(f => f.HpPer < 20);
            return friend != null;
            
        }

    }
}
