using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Scripts
{
    public class PetCatch:TaskBase
    {
        
        public override async Task DoOnce()
        {
            
            if (!Cg.Combat.IsFighting && Cg.Pets.Count == 5)
            {
                Stop();
                OnInfo("宠物已满，停止抓宠。");
                Cg.AutoWalk.Stop();
                return;
            }

            if (!Cg.Combat.IsFighting)
            {
                return;
            }


            if (Cg.Combat.IsPlayerTurn)
            {
                if (Cg.Combat.Units.Player.HpPer < 70 && Cg.Combat.Units.Enemys.Count < 3)
                {
                    var drug = Cg.Items.Drugs.FirstOrDefault();
                    if (drug != null)
                    {
                        await Cg.Combat.Cast(drug, Cg.Combat.Units.Player);
                        OnInfo($@"对自己使用{drug.Name}");
                        return;
                    }
                }

                if (Cg.Combat.Units.Player.HpPer < 30)
                {
                    var drug = Cg.Items.Drugs.FirstOrDefault();
                    if (drug != null)
                    {
                        await Cg.Combat.Cast(drug, Cg.Combat.Units.Player);
                        OnInfo($@"对自己使用{drug.Name}");
                        return;
                    }
                }

                if (Cg.Combat.Units.Pet.HpPer < 30)
                {
                    var drug = Cg.Items.Drugs.FirstOrDefault();
                    if (drug != null)
                    {
                        await Cg.Combat.Cast(drug, Cg.Combat.Units.Pet);
                        return;
                    }
                }



                var shuiLong = Cg.Combat.Units.Enemys.Find(e => e.Level == 1);
                if (shuiLong != null)
                {
                    OnInfo($@"遇到了Lv1 {shuiLong.Name},血量{shuiLong.HpMax}。");
                    if (shuiLong.HpMax > 123)
                    {
                        if (shuiLong.HpPer == 100)
                        {
                            var moFa = Cg.Player.Spells.FindExact("魔法");

                            if (moFa != null)
                            {
                                await Cg.Combat.Cast(moFa, shuiLong);
                                return;
                            }
                        }

                        var fengYinKa = Cg.Items.FindExact("封印卡");

                        if (fengYinKa != null)
                        {
                            await Cg.Combat.Cast(fengYinKa, shuiLong);
                            return;
                        }
                    }
                }
                await Cg.Combat.Escape();
            }

            if (Cg.Combat.IsPetTurn)
            {
                if (Cg.Combat.Units.Pet.HpPer < 70)
                {
                    var healSpell = Cg.Pets.CombatStatePet.Spells.HealSpell;
                    if (healSpell != null && Cg.Combat.Units.Pet.Mp > healSpell.MpCost)
                    {
                        await Cg.Combat.PetCast(healSpell, Cg.Combat.Units.GetRandomFrontEnemy());
                        return;
                    }
                }

                var spell = Cg.Pets.CombatStatePet.Spells.Find("防御");
                if (spell != null)
                {
                    await Cg.Combat.PetCast(spell);
                }
            }
        }

        public PetCatch(CrossGate cg) : base(cg)
        {
        }
    }
}
