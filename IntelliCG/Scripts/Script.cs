using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IntelliCG.Scripts
{
    public class Script:TaskBase
    {
       
        private MoveCondition DestinationCondition { get;  set; }
        private List<MoveCondition> GoConditions { get; set; } = new List<MoveCondition>();
        private List<MoveCondition> BackConditions { get; set; } = new List<MoveCondition>();
        private MoveCondition NurseCondition { get; set; }

        private bool _shouldBack;


        public override void Start()
        {
            _shouldBack = false;
            base.Start();
        }

        public override void Stop()
        {
            Cg.Cheater.BuBuYuDi = false;
            base.Stop();
        }
        public override async Task DoOnce()
        {
            if (Cg.Combat.IsFighting)
            {
                _shouldBack = Cg.Combat.Units.HasLFriendHpLowerThan();
                return;
            }
            if (Cg.Player.IsMoving)
            {
                return;
            }
            if (Cg.Player.HpPer < 30 || Cg.Player.MpPer < 10||_shouldBack)
            {
                //回村补血蓝
                Cg.Cheater.BuBuYuDi = false;
                await Go(BackConditions);
            }
            else
            {
                //去打怪
                await Go(GoConditions);
            }
        }

        public static Script GetNewTownInstance(CrossGate cg)
        {
            //57137 新村外，57138新村，57139新村医院

            var destinationCondition = new MoveCondition(351, 199, 57137, 352, 199);

            var g1 = new MoveCondition(57137, 351, 199);
           // g1.Description = "新城外任何一点满足，走到目的地351，199";
            var g2 = new MoveCondition(57138, 53, 49);
            //g2.Description = "新村内任意位置走到53，49，中间折返点";
            var g3 = new MoveCondition(53, 49, 57138, 51, 38);
            //g3.Description = "从折返点出村";
            var g4 = new MoveCondition(57139, 8, 14);
            //g4.Description = "医院里任意位置出医院";



            var nurseCondition = new MoveCondition(17, 12, 57139, 17, 10);
            //nurseCondition.Description = "补血补蓝";

            var b0 = new MoveCondition(57137, 351, 202);
            //b0.Description = "新村外任意位置回村";
            var b1 = new MoveCondition(351,202,57137, 351, 203);
            //b0.Description = "进村";
            var b2 = new MoveCondition(57138, 53, 49);
            //b2.Description = "新村内任意位置走到53，49，中间折返点";
            var b3 = new MoveCondition(53, 49, 57138, 58, 53);
            //b3.Description = "折返点进医院";
            var b4 = new MoveCondition(57139, 17, 12);
            //b4.Description = "医院里任意位置走到资深护士";

            var b5 = new MoveCondition(57150, 7, 17);

            var script = new Script(cg)
            {
                DestinationCondition = destinationCondition
            };
            script.GoConditions.Add(destinationCondition);
            script.GoConditions.Add(g1);
            script.GoConditions.Add(g2);
            script.GoConditions.Add(g3);
            script.GoConditions.Add(g4);

            script.NurseCondition = nurseCondition;
            script.BackConditions.Add(nurseCondition);
            script.BackConditions.Add(b1);
            script.BackConditions.Add(b2);
            script.BackConditions.Add(b3);
            script.BackConditions.Add(b4);
            script.BackConditions.Add(b0);
            script.BackConditions.Add(b5);
            return script;
        }



        private async Task Go(IEnumerable<MoveCondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (!condition.IsMatch(Cg.Player.X, Cg.Player.Y, Cg.Player.MapId)) continue;

                if (condition == NurseCondition)
                {
                    await Task.Delay(500);
                    OnInfo(@"补血。");
                    await Cg.Actions.RightClick(NurseCondition.TargetX, NurseCondition.TargetY);
                    await Cg.AutoNurse.DoOnce();
                    _shouldBack = false;
                    await Task.Delay(500);
                    return;
                }

                if (condition == DestinationCondition)
                {
                    OnInfo($@"原地遇敌。");
                    await Cg.Combat.Encounter();
                    return ;
                }

                if (Cg.Player.X == 351 && Cg.Player.Y == 202)
                {
                    OnInfo($@"进村等待。");
                    await Task.Delay(500);
                }
                
                OnInfo($@"走到{condition.TargetX},{condition.TargetY}。");
                await Cg.Actions.MoveTo(condition.TargetX, condition.TargetY);
                await Task.Delay(500);
            }
        }


        private Script(CrossGate cg) : base(cg)
        {
        }
    }
}
