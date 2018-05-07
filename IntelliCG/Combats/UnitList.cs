using System;
using System.Collections.Generic;
using System.Linq;
using IntelliCG.Players;
using MemoLib;

namespace IntelliCG.Combats
{
    public class UnitList : Base
    {
        private readonly Random _random=new Random();
        private  List<Unit>  _units=new List<Unit>();
        public List<Unit> Enemys { get; private set; }=new List<Unit>();
        public List<Unit> Friends { get; private set; }=new List<Unit>();

        public UnitList(Memo memo) : base(memo)
        {
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
            var units = new List<Unit>();
            var enemies=new List<Unit>();
            var friends=new List<Unit>();
            

            var roundMod = Memo.ReadInt(0x00645378, 1) + 1;
            var content = Memo.ReadString(0x00640230 + roundMod * 0x1000, 0x1000);
            if (content.Length > 20)
            {
                var entryList = content.Substring(4).Split('|').ToList();

                for (var i = 0; i < entryList.Count-1; i+=12)
                {
                    var unit = new Unit(entryList.GetRange(i, 11), this);
                    units.Add(unit);

                    if (unit.IsEnemy)
                    {
                        enemies.Add(unit);
                    }
                    else
                    {
                        friends.Add(unit);
                    }
                }
                
            }
            _units = units;
            Enemys = enemies;
            Friends = friends;
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

        public Unit GetLowestHpPerFriend()
        {
            return Friends.Where(f=>f.Hp>0).OrderBy(f => f.HpPer).First();
        }
        public Unit GetLowestHpFriendIncludingDead()
        {
            return Friends.OrderBy(f => f.Hp).First();
        }
        //血少于hpPer数量的强力位降序的第一个
        public Unit GetCrossFriend(int hpPer=70)
        {
            return Friends.OrderByDescending(f => f.CrossUnits.FindAll(cu => cu.HpPer < hpPer).Count).FirstOrDefault();
        }

        //找到强力位敌人最多的目标
        public Unit GetCrossEnemy()
        {
            return Enemys.OrderByDescending(f => f.CrossUnits.Count).FirstOrDefault();
        }

        public Unit GetEnemyByWeapon(Weapon weapon)
        {
            if (weapon == Weapon.HuiLi)
            {
                var front=Enemys.FindAll(e => e.IsInFront).Count;
                var back = Enemys.Count - front;
                return front > back ? Enemys.Find(e => e.IsInFront) : Enemys.Find(e => !e.IsInFront);
            }

            if (weapon == Weapon.Gong)
            {
                return GetRandomEnemy();
            }

            if (weapon == Weapon.XiaoDao)
            {
                var target=Enemys.Find(e => e.FrontUnit!=null);
                return target ?? GetRandomEnemy();
            }
            
            return GetRandomFrontEnemy();
            
        }

        public bool HasLFriendHpLowerThan(int hp=300)
        {
            var friend = Friends.Find(f => f.Hp < hp);
            return friend != null;
            
        }

        public bool HasLFriendHpPerLowerThan(int hpPer=30)
        {
            var friend = Friends.Find(f => f.HpPer < hpPer);
            return friend != null;
        }

        
    }
}
