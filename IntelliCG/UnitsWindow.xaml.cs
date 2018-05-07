using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IntelliCG.Combats;
using MahApps.Metro.Controls;

namespace IntelliCG
{
    /// <summary>
    /// UnitsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UnitsWindow
    {
        private readonly List<Label> _hpLabels;
        private readonly List<Label> _mpLabels;
        public CrossGate Cg { get; }

        public UnitsWindow(CrossGate cg)
        {
            InitializeComponent();
            Cg = cg;
            _hpLabels = new List<Label>();
            _mpLabels= new List<Label>();
            _hpLabels.Add(Hp10);
            _hpLabels.Add(Hp11);
            _hpLabels.Add(Hp12);
            _hpLabels.Add(Hp13);
            _hpLabels.Add(Hp14);
            _hpLabels.Add(Hp15);
            _hpLabels.Add(Hp16);
            _hpLabels.Add(Hp17);
            _hpLabels.Add(Hp18);
            _hpLabels.Add(Hp19);
            _mpLabels.Add(Mp10);
            _mpLabels.Add(Mp11);
            _mpLabels.Add(Mp12);
            _mpLabels.Add(Mp13);
            _mpLabels.Add(Mp14);
            _mpLabels.Add(Mp15);
            _mpLabels.Add(Mp16);
            _mpLabels.Add(Mp17);
            _mpLabels.Add(Mp18);
            _mpLabels.Add(Mp19);
            _enable = true;
            Refresh();
        }
        
        private bool _enable;
        private async void Refresh()
        {
            while (_enable)
            {
                _hpLabels.ForEach(l=>l.Content="/");
                _mpLabels.ForEach(l=>l.Content="/");
                if (Cg.Combat.IsFighting)
                {
                    Cg.Combat.Units.Read();
                    foreach (var unit in Cg.Combat.Units.Enemys)
                    {
                        _hpLabels[unit.Index-10].Content=unit.Hp+"/"+unit.HpMax;
                        _mpLabels[unit.Index-10].Content=unit.Mp+"/"+unit.MpMax;
                    }
                    Console.WriteLine(@"刷新显血窗口。");
                }
                await Task.Delay(1000);
            }
        }

        private void UnitsWindow_OnClosed(object sender, EventArgs e)
        {
            _enable = false;
        }
    }
}
