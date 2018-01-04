using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntelliCG.Combat;

namespace IntelliCG.Script
{
    public class Script
    {
        private Script(CrossGate cg)
        {
            Cg = cg;
            
            GoConditions = new List<MoveCondition>();
            BackConditions = new List<MoveCondition>();
        }
        private Thread Thread { get; set; }
        private CrossGate Cg { get; }
        private CancellationTokenSource Cts { get; set; }
        public MoveCondition DestinationCondition { get; private set; }
        public List<MoveCondition> GoConditions { get; set; }
        public List<MoveCondition> BackConditions { get; set; }
        public MoveCondition NurseCondition { get; set; }

        

        public bool EnableScript
        {
            set
            {
                if (value)
                {
                    Cts = new CancellationTokenSource();
                    Thread = new Thread(Logic);
                    Thread.Start();
                    Console.WriteLine(@"Start script thread.");
                }
                else
                {
                    Cts?.Cancel();
                }
            }
            get => Thread?.ThreadState == ThreadState.Running || Thread?.ThreadState == ThreadState.WaitSleepJoin;
        }

        private void Logic()
        {
            while (!Cts.IsCancellationRequested)
            {
                if (Cg.Player.IsMoving || Cg.Combat.IsFighting)
                {
                    continue;
                }
                if (Cg.Player.HpPer<30||Cg.Player.MpPer<10)
                {
                    //回村补血蓝
                    Cg.Cheat.BuBuYuDi = false;
                    Go(BackConditions);
                }
                else
                {
                    //去打怪
                    
                    Go(GoConditions);
                }
                Thread.Sleep(1000);
            }

        }

        public static Script GetInstance(CrossGate cg)
        {
            //57137 新村外，57138新村，57139新村医院

            var destinationCondition = new MoveCondition(351, 192, 57137, 352, 192);

            var g1 = new MoveCondition(57137, 351, 192);
            g1.Description = "新城外任何一点满足，走到目的地351，192";
            var g2 = new MoveCondition(57138, 53, 49);
            g2.Description = "新村内任意位置走到53，49，中间折返点";
            var g3 = new MoveCondition(53, 49, 57138, 51, 38);
            g3.Description = "从折返点出村";
            var g4 = new MoveCondition(57139, 8, 14);
            g4.Description = "医院里任意位置出医院";



            var nurseCondition = new MoveCondition(17, 12, 57139, 17, 10);
            nurseCondition.Description = "补血补蓝";
            var b1 = new MoveCondition(57137, 351, 203);
            b1.Description = "新村外任意位置回村";
            var b2 = new MoveCondition(57138, 53, 49);
            b2.Description = "新村内任意位置走到53，49，中间折返点";
            var b3 = new MoveCondition(53, 49, 57138, 58, 53);
            b3.Description = "折返点进医院";
            var b4 = new MoveCondition(57139, 17, 12);
            b4.Description = "医院里任意位置走到资深护士";

            
            var script = new Script(cg);
            script.DestinationCondition = destinationCondition;
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
            return script;
        }

       

        private void Go(IEnumerable<MoveCondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (condition.IsMatch(Cg.Player.X, Cg.Player.Y, Cg.Player.MapId))
                {

                    if (condition == NurseCondition)
                    {
                        Cg.RightClick(NurseCondition.TargetX, NurseCondition.TargetY);
                        Cg.NurseCall();
                        return;
                    }

                    if (condition==DestinationCondition)
                    {
                        Cg.Cheat.BuBuYuDi = true;
                    }
                    Cg.MoveTo(condition.TargetX, condition.TargetY);
                }
            }
        }


    }
}
