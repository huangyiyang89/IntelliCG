using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Controls;
using IntelliCG.Player;
using Spell = IntelliCG.Pet.Spell;

namespace IntelliCG.Combat
{
    public class AutoCombat
    {
        private CombatStrategy CombatStrategy { get; set; }
        private Thread Thread { get; set; }
        private CrossGate Cg { get; }
        private CancellationTokenSource Cts { get; set; }
        private int _walkFlag = 1;
        private int _startX;
        private int _startY;
        private bool _enableAutoWalk;
        public AutoCombat(CrossGate cg)
        {
            Cg = cg;
            CombatStrategy = new CombatStrategy(cg);
        }

        public bool EnableAutoCombat
        {
            set
            {
                if (value)
                {
                    Cts = new CancellationTokenSource();
                    Thread = new Thread(Logic);
                    Thread.Start();
                    Console.WriteLine(@"Start auto combat thread.");
                }
                else
                {
                    Cts?.Cancel();
                }
            }
            get => Thread?.ThreadState == ThreadState.Running || Thread?.ThreadState == ThreadState.WaitSleepJoin;
        }

        public bool EnableAutoWalk
        {
            get => _enableAutoWalk;
            set
            {
                _enableAutoWalk = value;
                if (!_enableAutoWalk) return;
                _startX = Cg.Player.X;
                _startY = Cg.Player.Y;

            }
        }

        public bool EnableFeedPet { get; set; }

        public delegate void MessageHandler(string content);
        public event MessageHandler Infomation;
        public bool AlwaysFirstSpell { get; set; }
        public bool EnableItems { get; set; }
        private bool _combatFlag;

        private void Walk()
        {
            Cg.MoveTo(_startX + _walkFlag, _startY);
            Thread.Sleep(500);
            _walkFlag = _walkFlag == 0 ? 1 : 0;
        }
        public void Logic()
        {
#if(!DEBUG)
            try
            {
#endif

                while (!Cts.IsCancellationRequested)
                {
                    if (Cg.Combat.Units.HasLowHpFriend()&&EnableAutoWalk&&!Cg.Combat.IsFighting)
                    {
                        EnableAutoWalk = false;
                        Cg.Cheat.BuBuYuDi = false;
                        Infomation?.Invoke("停止自动遇敌，有友方血量过低！");
                    }
                    if (!Cg.Combat.IsFighting && _combatFlag)
                    {
                        Infomation?.Invoke($@"----------战斗结束----------");
                        _combatFlag = false;
                    }
                    Thread.Sleep(500);
                    if (Cg.Combat.IsFighting)
                    {
                        if (_combatFlag == false)
                        {
                            Infomation?.Invoke($@"----------进入战斗----------.");
                            _combatFlag = true;
                        }

                        var isPlayerTurn = Cg.Combat.IsPlayerTurn;
                        var isPetTurn = Cg.Combat.IsPetTurn;

                        if (isPlayerTurn || isPetTurn)
                        {
                            Cg.Combat.Units.Read();
                        }

                        if (isPlayerTurn)
                        {
                            Infomation?.Invoke($@"人物行动回合");
                            if (Cg.Cheat.GaoSuZhanDou)
                            {
                                Thread.Sleep(3000);
                            }

                            var player = Cg.Combat.Units.Player;
                            var pet = Cg.Combat.Units.Pet;
                            var food = Cg.Items.Foods.FirstOrDefault();
                            var drug = Cg.Items.Drugs.FirstOrDefault();
                            var enemys = Cg.Combat.Units.Enemys;
                            var defaultTarget = Cg.Combat.Units.GetRandomEnemy(true);


                            //补血
                            if (player.HpPer < 30 && drug != null && EnableItems)
                            {
                                Cg.Combat.Cast(drug, player);
                                Infomation?.Invoke($@"对自己使用了血瓶:{drug.FirstLine}");
                                continue;
                            }

                            //宠物补血
                            if (pet != null && pet.HpPer < 30 && drug != null && EnableItems && EnableFeedPet)
                            {

                                Cg.Combat.Cast(drug, pet);
                                Infomation?.Invoke($@"对宠物使用了血瓶:{drug.FirstLine}");
                                continue;
                            }

                            //补蓝
                            if ((player.MpLose > 580 | player.MpPer < 10) && food != null && EnableItems)
                            {
                                Cg.Combat.Cast(food, player);
                                Infomation?.Invoke($@"对自己使用了料理:{food.FirstLine}");
                                continue;
                            }

                            //给宠补蓝
                            if (pet != null && (pet.MpLose < 580 || pet.MpPer < 10) && enemys.Count() < 4 && food != null && EnableItems && EnableFeedPet)
                            {
                                Cg.Combat.Cast(food, pet);
                                Infomation?.Invoke($@"对宠物使用料理:{food.FirstLine}");
                                continue;
                            }

                            //只用1技能
                            var customFirstSpell = Cg.Player.Spells.CustomFirstSpell;
                            if (AlwaysFirstSpell && player.Mp > customFirstSpell.MpCost)
                            {
                                Cg.Combat.Cast(customFirstSpell, defaultTarget);
                                Infomation?.Invoke($@"施放技能:{customFirstSpell.Name},{defaultTarget.Name},位置{defaultTarget.Index}");
                                continue;
                            }

                            CombatStrategy.PlayerEfficientAction();
                            Infomation?.Invoke(CombatStrategy.InfoMessage);
                            continue;
                        }

                        if (isPetTurn)
                        {
                            Infomation?.Invoke($@"宠物行动回合");
                            var pet = Cg.Pets.CombatStatePet;
                            var petHpPer = Cg.Combat.Units.Pet.HpPer;
                            var petMp = Cg.Combat.Units.Pet.Mp;
                            var efficientSpell = pet.Spells.EfficientSpell;
                            var defaultTarget = Cg.Combat.Units.GetRandomEnemy(!Cg.Combat.Units.Pet.IsInFront);
                            var healSpell = pet.Spells.HealSpell;
                            var noManaSpell = pet.Spells.NoManaSpell;

                            //吸血或明净
                            if (petHpPer < 70 && petMp >= healSpell.MpCost)
                            {
                                Cg.Combat.PetCast(healSpell, defaultTarget);
                                Infomation?.Invoke($@"宠物:{healSpell.Name},{defaultTarget.Name},位置{defaultTarget.Index}");
                                continue;
                            }
                            //护卫,放第一格
                            var huwei = pet.Spells["护卫"];
                            var lowHpFriend = Cg.Combat.Units.Friends.Find(f => f.HpPer < 30);
                            if (huwei != null && huwei.Index == 0 && lowHpFriend != null && petMp >= huwei.MpCost)
                            {
                                Cg.Combat.PetCast(huwei, lowHpFriend);
                                Infomation?.Invoke($@"宠物:{huwei.Name},{lowHpFriend.Name}");
                                continue;
                            }

                            //技能
                            if (efficientSpell != null && efficientSpell.Index == 0 && petMp >= efficientSpell.MpCost)
                            {
                                Cg.Combat.PetCast(efficientSpell, defaultTarget);
                                Infomation?.Invoke($@"宠物:{efficientSpell.Name},{defaultTarget.Name},位置{defaultTarget.Index}");
                                continue;
                            }
                            Cg.Combat.PetCast(noManaSpell, defaultTarget);
                            Infomation?.Invoke($@"宠物:{noManaSpell.Name},{defaultTarget.Name},位置{defaultTarget.Index}");
                        }
                    }
                    else
                    {
                        if (EnableAutoWalk)
                        {
                            Walk();
                        }
                    }
                }
#if (!DEBUG)
            }
            catch (Exception e)
            {
                Infomation?.Invoke(e.Message);
                Infomation?.Invoke(e.StackTrace);
            }
#endif
        }


    }


}

