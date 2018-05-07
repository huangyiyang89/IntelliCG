using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Scripts
{
    public class MoveScript:CgBase
    {
        public MoveScript(CrossGate cg) : base(cg)
        {
        }
        
        private static List<MoveCondition> _newTownNurse;
        public List<MoveCondition> NewTownNurse
        {
            get
            {
                if (_newTownNurse != null) return _newTownNurse;
                var moveConditions=new List<MoveCondition>();
                var b1 = new MoveCondition(57137, 351, 203);
                //b1.Description = "新村外任意位置回村";
                var b2 = new MoveCondition(57138, 53, 49);
                //b2.Description = "新村内任意位置走到53，49，中间折返点";
                var b3 = new MoveCondition(53, 49, 57138, 58, 53);
                //b3.Description = "折返点进医院";
                var b4 = new MoveCondition(57139, 17, 12);
                //b4.Description = "医院里任意位置走到资深护士";

                moveConditions.Add(b1);
                moveConditions.Add(b2);
                moveConditions.Add(b3);
                moveConditions.Add(b4);
                _newTownNurse = moveConditions;
                return _newTownNurse;
            }
        }

        private static List<MoveCondition> _newTownOutside;
        public static List<MoveCondition> NewTownOutside
        {
            get
            {
                if (_newTownOutside != null) return _newTownOutside;
                var moveConditions=new List<MoveCondition>();
                var g1 = new MoveCondition(57137, 351, 199);
                // g1.Description = "新城外任何一点满足，走到目的地351，199";
                var g2 = new MoveCondition(57138, 53, 49);
                //g2.Description = "新村内任意位置走到53，49，中间折返点";
                var g3 = new MoveCondition(53, 49, 57138, 51, 38);
                //g3.Description = "从折返点出村";
                var g4 = new MoveCondition(57139, 8, 14);
                //g4.Description = "医院里任意位置出医院";
                moveConditions.Add(g1);
                moveConditions.Add(g2);
                moveConditions.Add(g3);
                moveConditions.Add(g4);
                _newTownOutside = moveConditions;
                return _newTownOutside;
            }
        }

        private async Task GoTo(IEnumerable<MoveCondition> conditions,bool signOut=false)
        {

            if (Cg.Combat.IsFighting)
            {
                return;
            }
            if (Cg.Player.IsMoving)
            {
                return;
            }

            foreach (var condition in conditions)
            {
                if (!condition.IsMatch(Cg.Player.X, Cg.Player.Y, Cg.Player.MapId)) continue;
                
                await Cg.Actions.MoveTo(condition.TargetX, condition.TargetY);
            }

            if (signOut)
            {

            }
        }
    }
}
