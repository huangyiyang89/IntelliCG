using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IntelliCG.Combat
{
    public class Unit
    {
        //C|0|0|驯兽湿泛|19F25|0|9|A5|133|32|78|8000005|0|0|5|水龙蜥8D|18D59|0|9|4B|15A|9C|9C|6000009|0|0|A|哥布林|18DA8|0|1|67|67|46|46|3000001|0|0|
        //C|0|0|驯兽湿泛|19F25|0|9|A5|133|32|78|8000005|0|0|5|水龙蜥8D|18D59|0|9|4B|15A|9C|9C|6000009|0|0|A|哥布林|18DA8|0|2|7A|7A|4B|4B|3000001|0|0|F|哥布林|18DA8|0|2|71|71|48|48|3000001|0|0|

        public Unit(IReadOnlyList<string> dataList, UnitList units)
        {
            Position = dataList[0];
            Index = Convert.ToInt32(dataList[0], 16);
            Name = dataList[1];
            Hp = Convert.ToInt32(dataList[5], 16);
            HpMax = Convert.ToInt32(dataList[6], 16);
            Mp = Convert.ToInt32(dataList[7], 16);
            MpMax = Convert.ToInt32(dataList[8], 16);
           
            _units = units;

            Console.WriteLine($@"敌人,Index:{Index},Name:{Name},Hp:{Hp}/{HpMax},Mp:{Mp}/{MpMax},Position:{Position}");
        }

        private readonly UnitList _units;
        public int Index { get; }
        public string Position { get; }
        public int Hp { get; }
        public int HpMax { get; }
        public int HpLose => HpMax - Hp;

        public int Mp { get; }
        public int MpMax { get; }
        public int MpLose => MpMax - Mp;

        public int HpPer => Hp * 100 / HpMax;

        public int MpPer => Mp * 100 / MpMax;
        public string Name { get; }
        public bool Dead => Hp == 0;
       

        public bool IsEnemy => Index > 9;
        public bool IsFriend => Index < 10;

        public bool IsInFront => (Index > 4 && Index < 10) || Index >14;

        public Unit FrontUnit => IsInFront == false ? _units[Index + 5] : null;

        //14 12 10 11 13
        // 19 17 15 16 18
        // 
        // 9 7 5 6 8
        // 4 2 0 1 3

        public List<Unit> CrossUnits
        {
            get
            {
                var frontOrBehind = IsInFront ? Index - 5 : Index + 5;
                var type = Index % 5;
                switch (type)
                {
                    case 0:
                        return _units.FindAll(Index,Index + 1, Index + 2, frontOrBehind);
                    case 1:
                        return _units.FindAll(Index,Index - 1, Index + 2, frontOrBehind);
                    case 2:
                        return _units.FindAll(Index,Index - 2, Index + 2, frontOrBehind);
                    default:
                        return _units.FindAll(Index,Index - 2, frontOrBehind);
                }
            }
        }

    }
}
