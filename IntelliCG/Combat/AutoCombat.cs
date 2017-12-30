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



        public delegate void MessageHandler(string content);
        public event MessageHandler Infomation;
        public bool AlwaysFirstSpell { get; set; }
        public bool EnableItems { get; set; }


        private void Walk()
        {
            Cg.MoveTo(_startX + _walkFlag, _startY);
            Thread.Sleep(500);
            _walkFlag = _walkFlag == 0 ? 1 : 0;
        }
        public void Logic()
        {

            while (!Cts.IsCancellationRequested)
            {
                if (Cg.Combat.IsFighting)
                {
                    Console.WriteLine(@"战斗中ing...");

                    var isPlayerTurn = Cg.Combat.IsPlayerTurn;
                    var isPetTurn = Cg.Combat.IsPetTurn;

                    if (isPlayerTurn || isPetTurn)
                    {
                        Cg.Combat.Units.Read();
                    }

                    if (isPlayerTurn)
                    {
                        if (Cg.Cheat.GaoSuZhanDou)
                        {
                            Thread.Sleep(1500);
                        }

                        var player = Cg.Combat.Units.Player;
                        var pet = Cg.Combat.Units.Pet;
                        var food = Cg.Items.Foods.First();
                        var drug = Cg.Items.Drugs.First();
                        var enemys = Cg.Combat.Units.Enemys;
                        var defaultTarget = Cg.Combat.Units.GetRandomEnemy(true);


                        //补血
                        if (player.HpPer < 50 && drug != null && EnableItems)
                        {
                            Cg.Combat.UseItem(drug, player);
                            continue;
                        }

                        //宠物补血
                        if (pet != null && pet.HpPer < 50 && drug != null && EnableItems)
                        {
                            Cg.Combat.UseItem(drug, pet);
                            continue;
                        }

                        //补蓝
                        if ((player.MpLose < 580 | player.MpPer < 10) && enemys.Count() < 4 && food != null && EnableItems)
                        {
                            Cg.Combat.UseItem(food, player);
                            continue;
                        }

                        //给宠补蓝
                        if (pet != null && (pet.MpLose < 580 || pet.MpPer < 10) && enemys.Count() < 4 && food != null && EnableItems)
                        {
                            Cg.Combat.UseItem(food, pet);
                            continue;
                        }

                        var customFirstSpell = Cg.Player.Spells.CustomFirstSpell;
                        if (AlwaysFirstSpell && player.Mp > customFirstSpell.MpCost)
                        {
                            Cg.Combat.Cast(customFirstSpell, defaultTarget);
                            continue;
                        }

                        var efficientSpell = Cg.Player.Spells.EfficientSpell;
                        if (efficientSpell != null && player.Mp > efficientSpell.MpCost && enemys.Count > 2)
                        {
                            var useLowLevel = 9;
                            if (efficientSpell.Is("乱射"))
                            {
                                useLowLevel = enemys.Count * 2 - 3;
                            }
                            if (efficientSpell.Is("气功"))
                            {
                                switch (enemys.Count)
                                {
                                    case 3:
                                        useLowLevel = 4;
                                        break;
                                    case 4:
                                        useLowLevel = 5;
                                        break;
                                    default:
                                        useLowLevel = 9;
                                        break;
                                }
                            }
                            Cg.Combat.Cast(efficientSpell, defaultTarget, useLowLevel);
                            continue;
                        }

                        Cg.Combat.Cast(defaultTarget);
                    }

                    if (isPetTurn)
                    {
                        var petHpPer = Cg.Combat.Units.Pet.HpPer;
                        var petMp = Cg.Combat.Units.Pet.Mp;
                        var efficientSpell = Cg.Pets.CombatStatePet.Spells.EfficientSpell;
                        var defaultTarget = Cg.Combat.Units.GetRandomEnemy(!Cg.Combat.Units.Pet.IsInFront);
                        var healSpell = Cg.Pets.CombatStatePet.Spells.HealSpell;
                        var noManaSpell = Cg.Pets.CombatStatePet.Spells.NoManaSpell;

                        //吸血或明净
                        if (petHpPer < 80 && petMp >= healSpell.MpCost)
                        {
                            Cg.Combat.PetCast(healSpell, defaultTarget);
                            continue;
                        }
                        //护卫
                        var huwei = Cg.Pets.CombatStatePet.Spells["护卫"];
                        var lowHpFriend = Cg.Combat.Units.Friends.Find(f => f.HpPer < 30);
                        if (huwei != null && lowHpFriend != null && petMp >= huwei.MpCost)
                        {
                            Cg.Combat.PetCast(huwei, lowHpFriend);
                            continue;
                        }

                        //技能
                        if (efficientSpell != null && petMp >= efficientSpell.MpCost)
                        {
                            Cg.Combat.PetCast(efficientSpell, defaultTarget);
                            continue;
                        }
                        Cg.Combat.PetCast(noManaSpell, defaultTarget);
                    }
                }
                else
                {
                    if (EnableAutoWalk)
                    {
                        Walk();
                    }
                }

                Thread.Sleep(500);
            }
            Console.WriteLine(@"Stop auto combat thread.");
        }

    }


}

