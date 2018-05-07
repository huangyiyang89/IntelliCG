using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IntelliCG.Cheat;
using IntelliCG.Combats;
using IntelliCG.Environments;
using IntelliCG.Items;
using IntelliCG.Pets;
using IntelliCG.Scripts;
using IntelliCG.Players;
using IntelliCG.Produces;
using MemoLib;

namespace IntelliCG
{
    public class CrossGate : Base
    {
        public AutoNurse AutoNurse { get; }
        public Player Player { get; }
        public Cheater Cheater { get; }
        public Script Script { get; }
        public Actions Actions { get; }
        public AutoCombat AutoCombat { get; }
        public AutoWalk AutoWalk { get; }
        public ItemList Items { get; }
        public Producer Producer { get; set; }
        public Combat Combat { get; }
        public PetCatch PetCatch { get; }
        public PetList Pets { get; }
        public StuffList Stuffs { get; }
        public ContactList Contacts { get; }
        public Poster Poster { get; }
        public AutoCure AutoCure { get; }

        public AutoFood AutoFood { get; }

        public AutoChange AutoChange { get; }

        public CrossGate(Memo memo) : base(memo)
        {
            
            Actions = new Actions(memo);
            Cheater = new Cheater(memo);
            Player = new Player(memo);
            Stuffs=new StuffList(memo);
            
            Combat = new Combat(this);
            AutoCure=new AutoCure(this);
            Contacts=new ContactList(memo);
            Pets = new PetList(memo);
            Items = new ItemList(this);
            AutoFood=new AutoFood(this);
            AutoNurse = new AutoNurse(this);
            PetCatch=new PetCatch(this);
            Producer=new Producer(this);
            AutoCombat = new AutoCombat(this);
            AutoWalk = new AutoWalk(this);
            Script = Script.GetNewTownInstance(this);
            Poster=new Poster(this);
            AutoChange=new AutoChange(this);
            Cheater.YiDongJiaSu = true;
        }

       
        
        
        public void Close()
        {
            Console.WriteLine(@"CrossGate.Close()");
            AutoNurse.Stop();
            Cheater.Close();
            Memo.Close();
        }

        ~CrossGate()
        {
            Console.WriteLine(@"~CrossGate()");
        }
    }
}
