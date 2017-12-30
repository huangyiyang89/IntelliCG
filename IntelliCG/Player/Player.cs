using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Player
{
    

    public class Player:Base
    {




        public SpellList Spells { get; }

        public Player(int hwnd) : base(hwnd)
        {
            
            Spells=new SpellList(hwnd);
        }

        public string Name => Dm.ReadString(Hwnd, "00E150B0", 0, 14);

        public Job Job
        {
            get
            {
                var jobName=Dm.ReadString(Hwnd, "00E16420", 0, 20);
                if (jobName.Contains("弓")) return Job.Gongshou;
                if (jobName.Contains("格")) return Job.Gedou;
                return Job.Others;
            }
        }

        public int X => Convert.ToInt32(Dm.ReadFloat(Hwnd, "00C1EF10")/64);
        public int Y => Convert.ToInt32(Dm.ReadFloat(Hwnd, "00C20384")/64);

        //玩家签名
        public string Autograph => Dm.ReadString(Hwnd, "06000159", 0, 30);
    }
}
