using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Scripts
{
    /// <summary>
    /// 在某个矩形范围内，目的地为TargetX,TargetY
    /// </summary>
    public class MoveCondition
    {
        public MoveCondition(int x1,int y1,int x2,int y2,int mapId,int targetX,int targetY)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            MapId = mapId;
            TargetX = targetX;
            TargetY = targetY;
        }
        public MoveCondition(int x, int y,int mapId, int targetX, int targetY)
        {
            X1 = x;
            Y1 = y;
            X2 = x;
            Y2 = y;
            MapId = mapId;
            TargetX = targetX;
            TargetY = targetY;
        }
        public MoveCondition(int mapId, int targetX, int targetY)
        {
            X1 = 0;
            Y1 = 0;
            X2 = 999;
            Y2 = 999;
            MapId = mapId;
            TargetX = targetX;
            TargetY = targetY;
        }

        //矩形范围
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int MapId { get; set; }

        //目的地
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public string Description { get; set; }

        
        //在矩形范围内
        public bool IsMatch(int x,int y,int mapId)
        {
            return (x <= X1 && x >= X2 || x >= X1 && x <= X2) &&
                   (y <= Y1 && y >= Y2 || y >= Y1 && y <= Y2) &&
                   MapId == mapId &&
                   !(x == TargetX && y == TargetY);

        }

        //在目的地
        public bool IsArrived(int x,int y)
        {
            return TargetX == x && TargetY == y;
        }
        
    }
}
