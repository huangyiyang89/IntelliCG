using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IntelliCG.Extensions;

namespace IntelliCG.Scripts
{
    public class AutoFood : TaskBase
    {
        public AutoFood(CrossGate cg) : base(cg)
        {
        }

        private long _lastTimer;

        public override async Task DoOnce()
        {
            if (Cg.Combat.IsFighting)
            {
                return;
            }

            var food = Cg.Items.Foods.FirstOrDefault();
            if (food != null && Cg.Player.MpPer < 60&&CanEat())
            {
                var str =
                    $@"L/w {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} {food.Index8.To62String()} 0 ";
                await Cg.Actions.DecodeSend(str);
                _lastTimer = DateTime.Now.Ticks;
                Console.WriteLine(str);
                OnInfo($@"吃了个{food.Name}。");
            }

            await Task.Delay(1000);
        }

        private bool CanEat()
        {
            var dt = (DateTime.Now.Ticks - _lastTimer);
            var s=TimeSpan.FromTicks(dt);
            return s.TotalSeconds > 60;
        }
    }
}
