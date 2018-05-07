using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Scripts
{
    public class AutoWalk:TaskBase
    {

        
        public bool StopOnHp { get; set; }
        
        private bool _combatFlag;
        public override async Task DoOnce()
        {
            if (Cg.Combat.IsFighting)
            {
                _combatFlag = true;
                return;
            }
            
            if (_combatFlag)
            {
                //结束战斗
                if (StopOnHp&&Cg.Combat.Units.HasLFriendHpPerLowerThan(20))
                {
                    _combatFlag = false;
                    Stop();
                    OnInfo("有队友血量过低，停止自动遇敌。");
                    return;
                }
            }
            await Task.Delay(100);
            _combatFlag = false;
            //_walkFlag = _walkFlag == 0 ? 1 : 0;
            await Cg.Combat.Encounter();
            //await Cg.Actions.MoveTo(X+_walkFlag,Y);
            
        }

        public AutoWalk(CrossGate cg):base(cg)
        {
            
        }
        
    }
}
