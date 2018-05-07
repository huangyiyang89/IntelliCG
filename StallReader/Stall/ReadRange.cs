using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Stall
{

    //  北  东
    // 西  南
    public enum RangeDirection
    {
        N2S,E2W,S2N,W2E
    }

    public class ReadRange
    {
        public ReadRange(int x,int y,RangeDirection direction)
        {
            X = x;
            Y = y;
            Range = 11;
            Direction = direction;
        }
        public ReadRange(int x,int y,int range,RangeDirection direction)
        {
            X = x;
            Y = y;
            Range = range;
            Direction = direction;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Range { get; set; }
        public RangeDirection Direction { get;set; }
    }
}
