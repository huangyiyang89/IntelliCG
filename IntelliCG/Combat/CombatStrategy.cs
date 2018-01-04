using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.Player;

namespace IntelliCG.Combat
{
    public class CombatStrategy
    {
        public CrossGate Cg;
        
        public CombatStrategy(CrossGate cg)
        {
            Cg = cg;
        }

        public string InfoMessage { get; set; }
        public void  PlayerEfficientAction()
        {
            InfoMessage = "";
            if (Cg.Combat.HasUsedSpell)
            {
                Cg.Combat.Cast(Cg.Combat.Units.GetTargetByWeapon(Cg.Player.Weapon));
                InfoMessage = $@"已使用过技能，普攻。";
                return;
            }
            if (Cg.Player.Job == Job.Gongshou)
            {
                var weapon = Cg.Player.Weapon;
                if (weapon != Weapon.Gong)
                {
                    Cg.Combat.Cast(Cg.Combat.Units.GetRandomFrontEnemy());
                    InfoMessage = $@"没拿弓，使用普通攻击。";
                    return;
                }
                
                var luanShe = Cg.Player.Spells["乱射"];
                if (luanShe != null&& Cg.Combat.Units.Enemys.Count>2)
                {
                    var useLevel=Cg.Combat.Units.Enemys.Count * 2 - 3;
                    if (Cg.Player.Mp > luanShe.MpCost)
                    {
                        Cg.Combat.Cast(luanShe, Cg.Combat.Units.Enemys.First(), useLevel);
                        InfoMessage = $@"使用乱射。";
                        return;
                    }
                    InfoMessage += "魔法不足，";
                }
                Cg.Combat.Cast(Cg.Combat.Units.GetRandomEnemy());
                InfoMessage += "使用普通攻击。";
                return;
            }
            if(Cg.Player.Job == Job.Gedou)
            {
                var qiGongDan = Cg.Player.Spells["气功"];
                if (qiGongDan != null && Cg.Combat.Units.Enemys.Count > 2)
                {
                    var useLevel=9;
                    switch (Cg.Combat.Units.Enemys.Count)
                    {
                        case 3:
                            useLevel = 4;
                            break;
                        case 4:
                            useLevel = 5;
                            break;
                        default:
                            useLevel = 9;
                            break;
                    }
                    if (Cg.Player.Mp > qiGongDan.MpCost)
                    {
                        Cg.Combat.Cast(qiGongDan, Cg.Combat.Units.GetRandomEnemy(), useLevel);
                        InfoMessage = $@"使用气功蛋。";
                        return;
                    }
                    InfoMessage += "魔法不足，";
                }

                Cg.Combat.Cast(Cg.Combat.Units.GetRandomFrontEnemy());
                InfoMessage += "使用普通攻击。";
                return;
            }
            if(Cg.Player.Job == Job.Chuanjiao)
            {
                var loseHpFriendCount=Cg.Combat.Units.Friends.FindAll(f => f.HpPer < 80).Count;
                //强力位置
                var crossTarget = Cg.Combat.Units.Friends.OrderByDescending(f => f.CrossUnits.FindAll(cu => cu.HpPer < 70).Count).FirstOrDefault();
                var crossCount = crossTarget?.CrossUnits.FindAll(c => c.HpPer < 70).Count ?? 0;
                //单体
                var singleTarget= Cg.Combat.Units.Friends.OrderBy(f => f.HpPer).FirstOrDefault();

                if (crossCount>loseHpFriendCount/2
                    &&loseHpFriendCount>1
                    )
                {
                    //强力
                    var qiangLi = Cg.Player.Spells["强力补血"];
                    if (qiangLi != null&&Cg.Player.Mp>qiangLi.MpCost)
                    {
                        Cg.Combat.Cast(qiangLi,crossTarget);
                        InfoMessage = $@"使用强力补血";
                        return;
                    }
                    InfoMessage += "使用强力补血，魔法不足。";
                }

                if (loseHpFriendCount > 2)
                {
                    //超强
                    var chaoQiang = Cg.Player.Spells["超强补血"];
                    if (chaoQiang != null && Cg.Player.Mp > chaoQiang.MpCost)
                    {
                        Cg.Combat.Cast(chaoQiang,Cg.Combat.Units.Player);
                        InfoMessage = $@"使用超强补血";
                        return;
                    }
                    InfoMessage += "使用超强补血，魔法不足。";
                }

                if (singleTarget?.HpPer < 95)
                {
                    //单体
                    var danTi = Cg.Player.Spells.FindEqualsFirst("补血魔法");
                    if (danTi != null && Cg.Player.Mp > danTi.MpCost)
                    {
                        var useLevel = (100-singleTarget.HpPer)/5;
                        Cg.Combat.Cast(danTi,singleTarget,useLevel);
                        InfoMessage = $@"使用补血魔法";
                        return;
                    }
                    InfoMessage += "使用单体补血，魔法不足。";
                }
                //普攻
                Cg.Combat.Cast(Cg.Combat.Units.GetTargetByWeapon(Cg.Player.Weapon));
                InfoMessage += "使用普通攻击。";
                return;
            }
            if (Cg.Player.Job == Job.Mofashi)
            {
                var enemysCount = Cg.Combat.Units.Enemys.Count;
                var crossTarget = Cg.Combat.Units.Enemys.OrderByDescending(f => f.CrossUnits.Count).FirstOrDefault();
                var crossCount = crossTarget?.CrossUnits.Count ?? 0;
                InfoMessage = $@"敌人数量{enemysCount}个，强力位数量{crossCount}个，";
                if (enemysCount > 6||
                    crossCount<=2&&enemysCount>3
                    )
                {
                    //超强
                    var chaoQiang = Cg.Player.Spells.FindEqualsFirst("超强陨石魔法", "超强冰冻魔法", "超强火焰魔法", "超强风刃魔法");
                    if (chaoQiang != null && Cg.Player.Mp > chaoQiang.MpCost)
                    {
                        Cg.Combat.Cast(chaoQiang,Cg.Combat.Units.Enemys.First());
                        InfoMessage += "使用超强魔法。";
                        return;
                    }
                    InfoMessage += "使用超强魔法，魔法不足。";
                }

                if (crossCount > 2)
                {
                    //强力
                    var qiangLi = Cg.Player.Spells.FindEqualsFirst("强力陨石魔法", "强力冰冻魔法", "强力火焰魔法", "强力风刃魔法");
                    if (qiangLi != null && Cg.Player.Mp > qiangLi.MpCost)
                    {
                        Cg.Combat.Cast(qiangLi,crossTarget);
                        InfoMessage += "使用强力魔法。";
                        return;
                    }
                    InfoMessage += "使用强力魔法，魔法不足。";
                }
                var danTi = Cg.Player.Spells.FindEqualsFirst("陨石魔法","冰冻魔法","火焰魔法","风刃魔法");
                if (danTi != null && Cg.Player.Mp > danTi.MpCost)
                {
                    Cg.Combat.Cast(danTi,Cg.Combat.Units.GetRandomEnemy());
                    InfoMessage += "使用单体魔法。";
                    return;
                }
                InfoMessage += "使用单体魔法，魔法不足。";
                //普攻
                Cg.Combat.Cast(Cg.Combat.Units.GetRandomFrontEnemy());
                InfoMessage += "使用普通攻击。";
                return;
            }

            if (Cg.Player.Job == Job.Wushi)
            {
                if (Cg.Combat.Round == 0)
                {
                    var chaoQiang = Cg.Player.Spells["超强恢复魔法"];
                    if (chaoQiang != null && Cg.Player.Mp > chaoQiang.MpCost)
                    {
                        Cg.Combat.Cast(chaoQiang, Cg.Combat.Units.Player);
                        InfoMessage += "使用超强恢复魔法。";
                        return;
                    }
                    InfoMessage += "使用超强恢复魔法，魔法不足。";
                }
                Cg.Combat.Cast(Cg.Combat.Units.GetTargetByWeapon(Cg.Player.Weapon));
                InfoMessage += "使用普通攻击。";
                return;
            }
            else
            {
                Cg.Combat.Cast(Cg.Combat.Units.GetTargetByWeapon(Cg.Player.Weapon));
                InfoMessage += "使用普通攻击。";
                return;
            }


        }



    }
}
