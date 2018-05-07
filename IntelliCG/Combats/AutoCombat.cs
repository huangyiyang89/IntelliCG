using System;
using System.Linq;
using System.Threading.Tasks;
using IntelliCG.Items;
using IntelliCG.Players;
using IntelliCG.Pets;
namespace IntelliCG.Combats
{
    public class AutoCombat : TaskBase
    {
        public AutoCombat(CrossGate cg) : base(cg)
        {
        }
        public bool EnableFeedPet { get; set; }

        public bool AlwaysCastFirstSpell { get; set; }
        public bool EnableItems { get; set; }
        private bool _combatFlag;

        public int GaoSuDelay { get; set; } = 4000;


        public override async Task DoOnce()
        {
            if (!Cg.Combat.IsFighting)
            {
                if (!_combatFlag) return;
                OnInfo($@"----------结束战斗----------");
                _combatFlag = false;
                return;

            }
            if (_combatFlag == false)
            {
                OnInfo($@"----------进入战斗----------");
                _combatFlag = true;
            }



            if (Cg.Combat.IsPlayerTurn)
            {
                
                if (Cg.Combat.IsPlayerFirstTurn)
                {
                    if (Cg.Cheater.GaoSuZhanDou)
                    {
                        if (Cg.Combat.Round != 0)
                        {
                            await Task.Delay(GaoSuDelay);
                        }
                    }

                    
                    //await Task.Delay(3000);
                }

                if (Cg.Combat.IsPlayerSecondTurn)
                {
                    await PlayerCast();
                    return;
                }
                Cg.Combat.Units.Read();

                var player = Cg.Combat.Units.Player;
                var pet = Cg.Combat.Units.Pet;
                var food = Cg.Items.Foods.FirstOrDefault();
                var drug = Cg.Items.Drugs.FirstOrDefault();
                var enemys = Cg.Combat.Units.Enemys;


                //补血
                if (player.HpPer < 40 && drug != null && EnableItems)
                {
                    await Cg.Combat.Cast(drug, player);
                    //Info?.Invoke(this,new MessageEventArgs($@"对自己使用了血瓶:{drug.FirstLine}"));
                    return;
                }

                //宠物补血
                if (pet != null && pet.HpPer < 40 && drug != null && EnableItems && EnableFeedPet)
                {

                    await Cg.Combat.Cast(drug, pet);
                    //Info?.Invoke(this,new MessageEventArgs($@"对宠物使用了血瓶:{drug.FirstLine}"));
                    return;
                }

                //补蓝
                if ((player.MpPer < 10) && food != null && EnableItems)
                {
                    await Cg.Combat.Cast(food, player);
                    OnInfo("人物行动：对自己使用了料理。");
                    return;
                }

                //给宠补蓝
                if (pet != null && (pet.MpPer < 10) && enemys.Count() < 4 && food != null && EnableItems && EnableFeedPet)
                {
                    await Cg.Combat.Cast(food, pet);
                    OnInfo("人物行动：对宠物使用了料理。");
                    return;
                }

                //只用1技能
                var customFirstSpell = Cg.Player.Spells.CustomFirstSpell;
                if (AlwaysCastFirstSpell && !Cg.Combat.Units.HasLFriendHpLowerThan())
                {
                    if (await PlayerCast(customFirstSpell))
                    {
                        return;
                    }
                }

                if (Cg.Player.Job == Job.Gongshou)
                {
                    if (await PlayerCast("乱射"))
                    {
                        return;
                    }
                    await PlayerCast();
                    return;
                }
                if (Cg.Player.Job == Job.Gedou)
                {
                    if (await PlayerCast("气功弹"))
                    {
                        return;
                    }
                    await PlayerCast();
                    return;
                }
                if (Cg.Player.Job == Job.Chuanjiao)
                {
                    var loseHpFriendCount = Cg.Combat.Units.Friends.FindAll(f => f.HpPer < 80).Count;
                    var crossFriend = Cg.Combat.Units.GetCrossFriend();
                    var crossCount = crossFriend.CrossUnits.FindAll(c => c.HpPer < 70).Count;

                    if (crossCount > loseHpFriendCount / 2
                        && loseHpFriendCount > 1
                        )
                    {
                        //强力
                        if (await PlayerCast("强力补血魔法", crossFriend))
                        {
                            return;
                        }
                    }
                    if (loseHpFriendCount > 2)
                    {
                        //超强
                        if (await PlayerCast("超强补血魔法"))
                        {
                            return;
                        }
                    }
                    var singleTarget = Cg.Combat.Units.GetLowestHpPerFriend();
                    if (singleTarget.HpPer>90&&player.MpLose > 600 && food != null && EnableItems)
                    {
                        await Cg.Combat.Cast(food, player);
                        //Info?.Invoke(this,new MessageEventArgs($@"对自己使用了料理:{food.FirstLine}"));
                        return;
                    }
                    if (singleTarget?.HpPer < 70)
                    {
                        //单体
                        if (await PlayerCast("补血魔法"))
                        {
                            return;
                        }
                    }

                    if (await Food())
                    {
                        return;
                    };
                    await PlayerCast();
                    return;
                }
                if (Cg.Player.Job == Job.Mofashi)
                {
                    var enemysCount = Cg.Combat.Units.Enemys.Count;
                    var crossEnemy = Cg.Combat.Units.GetCrossEnemy();
                    var crossCount = crossEnemy?.CrossUnits.Count ?? 0;

                    if (enemysCount > 6 ||
                        crossCount <= 2 && enemysCount > 3
                        )
                    {
                        //超强
                        var chaoQiang = Cg.Player.Spells.FindTheFirst("超强陨石魔法", "超强冰冻魔法", "超强火焰魔法", "超强风刃魔法");
                        if (await PlayerCast(chaoQiang))
                        {
                            return;
                        }
                    }

                    if (crossCount > 2)
                    {
                        //强力
                        var qiangLi = Cg.Player.Spells.FindTheFirst("强力陨石魔法", "强力冰冻魔法", "强力火焰魔法", "强力风刃魔法");
                        if (await PlayerCast(qiangLi, crossEnemy))
                        {
                            return;
                        }
                    }
                    var danTi = Cg.Player.Spells.FindTheFirst("陨石魔法", "冰冻魔法", "火焰魔法", "风刃魔法");
                    if (await PlayerCast(danTi))
                    {
                        return;
                    }
                    
                    if (await Food())
                    {
                        return;
                    };
                    //普攻
                    await Cg.Combat.Cast(Cg.Combat.Units.GetRandomFrontEnemy());

                    return;
                }
                if (Cg.Player.Job == Job.Wushi)
                {
                    if (Cg.Combat.Round == 0)
                    {
                        var loseHpFriendCount = Cg.Combat.Units.Friends.FindAll(f => f.HpPer < 90).Count;
                        //强力位置
                        var crossFriend = Cg.Combat.Units.GetCrossFriend();
                        var crossCount = crossFriend?.CrossUnits.FindAll(c => c.HpPer < 70).Count ?? 0;
                        //单体
                        var singleTarget = Cg.Combat.Units.Friends.OrderBy(f => f.HpPer).FirstOrDefault();


                        if (singleTarget?.HpPer < 30)
                        {
                            if (drug != null && EnableItems)
                            {
                                await Cg.Combat.Cast(drug, singleTarget);
                                //Info?.Invoke(this,new MessageEventArgs($@"对{singleTarget.Name}使用了血瓶:{drug.FirstLine}"));
                                return;
                            }
                            if (await PlayerCast("补血魔法", singleTarget))
                            {
                                return;
                            }
                        }

                        if (crossCount > loseHpFriendCount / 2 && loseHpFriendCount > 1
                            )
                        {
                            if (await PlayerCast("强力恢复魔法", crossFriend))
                            {
                                return;
                            }
                        }
                        if (loseHpFriendCount > 2)
                        {
                            if (await PlayerCast("超强恢复魔法"))
                            {
                                return;
                            }
                        }

                        if (singleTarget?.HpPer < 90)
                        {
                            if (await PlayerCast("恢复魔法", singleTarget))
                            {
                                return;
                            }
                        }
                    }

                    
                    
                    if (await Food())
                    {
                        return;
                    };
                    await PlayerCast();
                    return;
                }


                else
                {
                    if (await PlayerCast("乾坤一掷"))
                    {
                        return;
                    }
                    await PlayerCast(); return;
                }
            }

            if (Cg.Combat.IsPetTurn)
            {
                Cg.Combat.Units.Read();
                var pet = Cg.Pets.CombatStatePet;


                //护卫,放第一格
                var huwei = pet.Spells["护卫"];
                var lowHpFriend = Cg.Combat.Units.Friends.Find(f => f.HpPer < 50);
                if (huwei != null && lowHpFriend != null && huwei.Index == 0)
                {
                    if (await PetCast(huwei))
                    {
                        return;
                    }
                }
                //吸血或明净
                var healSpell = pet.Spells.HealSpell;
                var petHpPer = Cg.Combat.Units.Pet.HpPer;
                if (petHpPer < 70)
                {
                    if (await PetCast(healSpell))
                    {
                        return;
                    }
                }

                //强力魔法
                var strongSpell = pet.Spells.StrongSpell;
                var target = Cg.Combat.Units.GetCrossEnemy();
                if (target.CrossUnits.Count > 2)
                {
                    if (await PetCast(strongSpell))
                    {
                        return;
                    }
                }
                //技能
                var efficientSpell = pet.Spells.EfficientSpell;
                if (efficientSpell != null && efficientSpell.Index == 0)
                {
                    if (await PetCast(efficientSpell))
                    {
                        return;
                    }
                }
                //普攻或防御
                var noManaSpell = pet.Spells.NoManaSpell;
                await PetCast(noManaSpell);
            }
        }



       



        public async Task PlayerCast(Unit target = null)
        {
            if (target != null)
            {
                await Cg.Combat.Cast(target);
                return;
            }
            var t = Cg.Combat.Units.Player.IsInFront
                ? Cg.Combat.Units.GetRandomEnemy()
                : Cg.Combat.Units.GetEnemyByWeapon(Cg.Player.Weapon);
            await Cg.Combat.Cast(t);
            OnInfo($@"人物行动：普攻，{t.Name}。");
            //Info?.Invoke(this,new MessageEventArgs();
        }
        public async Task<bool> PlayerCast(Spell spell, Unit target = null)
        {
            
            if (spell == null)
            {
                return false;
            }

            if (Cg.Combat.IsPlayerSecondTurn)
            {
                return false;
            }
            if (spell.MpCost > Cg.Player.Mp)
            {
                OnInfo($@"人物行动：使用{spell.Name}，Mp不足。");
                return false;
            }
            OnInfo($@"人物行动：使用{spell.Name}");
            if (target != null)
            {
                await Cg.Combat.Cast(spell, target);
                return true;
            }
            if (spell.Name == "超强补血魔法" || spell.Name == "超强恢复魔法" || spell.Name == "洁净魔法")
            {
                //始终对自身施放
                await Cg.Combat.Cast(spell, Cg.Combat.Units.Player);
                return true;
            }

            if (spell.Name == "强力补血魔法" || spell.Name == "强力恢复魔法")
            {
                //找血少强力位
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetCrossFriend());
                return true;
            }

            if (spell.Name == "补血魔法")
            {
                var t = Cg.Combat.Units.GetLowestHpPerFriend();
                var useLevel = (100 - t.HpPer) / 7; await Cg.Combat.Cast(spell, t, useLevel);
                return true;
            }
            if (spell.Name == "恢复魔法")
            {
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetLowestHpPerFriend());
                return true;
            }

            if (spell.Name.Contains("超强"))
            {
                await Cg.Combat.Cast(spell, Cg.Combat.Units.Enemys.First());
                return true;
            }

            if (spell.Name.Contains("强力"))
            {
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetCrossEnemy());
                return true;
            }
            if (spell.Name.Contains("魔法") || spell.Name == "属性反转")
            {
                //随机目标
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetRandomEnemy());
                return true;
            }

            if (spell.Name == "气绝回复")
            {
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetLowestHpFriendIncludingDead());
                return true;
            }

            if (spell.Name.Contains("吸收") || spell.Name.Contains("反弹") || spell.Name == "护卫")
            {
                //血量最低友方
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetLowestHpPerFriend());
                return true;
            }

            if (spell.Name == "连击" || spell.Name == "诸刃" || spell.Name == "崩击" || spell.Name == "战栗袭心" || spell.Name == "混乱攻击")
            {
                if (Cg.Player.Weapon != Weapon.Others)
                {
                    return false;
                }
                var t = Cg.Combat.Units.Player.IsInFront
                    ? Cg.Combat.Units.GetRandomEnemy()
                    : Cg.Combat.Units.GetRandomFrontEnemy();
                await Cg.Combat.Cast(spell, t);
                return true;
            }



            if (spell.Name == "乾坤一掷")
            {
                var t = Cg.Combat.Units.Player.IsInFront
                    ? Cg.Combat.Units.GetRandomEnemy()
                    : Cg.Combat.Units.GetEnemyByWeapon(Cg.Player.Weapon);
                await Cg.Combat.Cast(spell, t);
                return true;
            }

            if (spell.Name == "反击" || spell.Name == "明镜止水" || spell.Name == "暗黑骑士之力" || spell.Name.Contains("祈祷"))
            {
                await Cg.Combat.Cast(spell);
                return true;
            }

            if (spell.Name == "气功弹")
            {
                int useLevel;
                

                switch (Cg.Combat.Units.Enemys.Count)
                {
                    case 1:
                        return false;
                    case 2:
                        return false;
                    case 3:
                        useLevel = 4;
                        break;
                    case 4:
                        useLevel = 7;
                        break;
                    default:
                        useLevel = 9;
                        break;
                }


                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetRandomEnemy(), useLevel);
                return true;
            }
            if (spell.Name == "乱射")
            {
                if (Cg.Player.Weapon != Weapon.Gong)
                {
                    return false;

                }

                int useLevel;
                var round = Cg.Combat.Round;
                var zudui = Cg.Combat.Units.Friends.Count > 7;
                switch (Cg.Combat.Units.Enemys.Count)
                {
                    case 1:
                        return false;
                    case 2:
                        if (round == 0||!zudui)
                        {
                            useLevel = 1;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 3:
                        if (round == 0||!zudui)
                        {
                            useLevel = 2;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 4:
                        if (round == 0||!zudui)
                        {
                            useLevel = 5;
                        }
                        else
                        {
                            useLevel = 2;
                        }
                        break;
                    default:
                        useLevel = 9;
                        break;
                }
                
                await Cg.Combat.Cast(spell, Cg.Combat.Units.GetRandomEnemy(), useLevel);
                return true;
            }

            return false;
        }
        public async Task<bool> PlayerCast(string spellName, Unit target = null)
        {
            var spell = Cg.Player.Spells.FindExact(spellName); return await PlayerCast(spell, target);

        }

        public async Task<bool> PetCast(string spellName, Unit target = null)
        {

            var spell = Cg.Pets.CombatStatePet.Spells.Find(spellName);
            return await PetCast(spell, target);
        }

        public async Task<bool> PetCast(PetSpell spell, Unit target = null)
        {
            if (spell == null)
            {
                return false;
            }

            if (spell.MpCost > Cg.Combat.Units.Pet.Mp)
            {
                OnInfo($@"宠物行动：使用{spell.Name}，Mp不足。");
                return false;
            }

            if (target != null)
            {
                await Cg.Combat.PetCast(spell, target);
                return true;
            }
            OnInfo($@"宠物行动：使用{spell.Name}。");
            if (spell.Name.Contains("护卫"))
            {
                await Cg.Combat.PetCast(spell, Cg.Combat.Units.GetLowestHpPerFriend());
                return true;
            }
            if (spell.Name.Contains("强力"))
            {
                await Cg.Combat.PetCast(spell, Cg.Combat.Units.GetCrossEnemy());
                return true;
            }
            if (spell.Name.Contains("魔法"))
            {
                await Cg.Combat.PetCast(spell, Cg.Combat.Units.GetRandomEnemy());
                return true;
            }


            if (Cg.Combat.Units.Pet.IsInFront)
            {
                await Cg.Combat.PetCast(spell, Cg.Combat.Units.GetRandomEnemy());
                return true;
            }
            else
            {
                await Cg.Combat.PetCast(spell, Cg.Combat.Units.GetRandomFrontEnemy());
                return true;
            }
        }

        public async Task<bool> Food()
        {
            var food = Cg.Items.Foods.FirstOrDefault();
            var player = Cg.Combat.Units.Player;
            if (food != null && EnableItems)
            {
                var foodAdd = 600;
                if (food.Name == "面包")
                {
                    foodAdd = 100;
                }
                if (player.MpLose > foodAdd)
                {
                    await Cg.Combat.Cast(food, player);
                    OnInfo("人物行动：对自己使用了料理。");
                    return true;
                }
            }
            return false;
        }
    }


}

