using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Scripts
{
    public class AutoCure:TaskBase
    {
        public AutoCure(CrossGate cg) : base(cg)
        {
        }

        

        public override async Task DoOnce()
        {
            if (Cg.Combat.IsFighting)
            {
                return;

            }
            if (Cg.Player.InJury > 0 && Cg.Player.InJury < 26)
            {
                var spell = Cg.Player.Spells.FindExact("治疗");

                if (spell == null)
                {
                    Stop();
                    OnInfo("未学习治疗技能。");
                    return;
                }
                
                if (Cg.Player.Mp > spell.MpCost)
                {
                    await Cg.Actions.DecodeSend("E&: "+spell.Index+" ");
                    await Task.Delay(500);
                    await Cg.Actions.DecodeSend("-C6 "+spell.Index+" 0 0 ");
                    OnInfo("对自己进行了治疗。");
                    await Task.Delay(1000);
                    await ReWork();
                }
            }

            
            await Task.Delay(1000);
        }



        public async Task ReWork()
        {
            Cg.Memo.WriteBytes(0x004EBDA5, "90-90");
            Cg.Memo.WriteBytes(0x004EBE8C, "90-90-90-90-90-90");
            await Task.Delay(100);
            Cg.Memo.WriteBytes(0x004EBDA5, "74-4A");
            Cg.Memo.WriteBytes(0x004EBE8C, "0F-84-99-04-00-00");
        }
    }
}
