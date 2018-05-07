//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CgData.Models;

//namespace IntelliCG.Record
//{
//    public class Saver:CgBase
//    {
//        public Saver(CrossGate cg) : base(cg)
//        {
//        }


//        public void Save()
//        {
//            var store = new Store();


//            store.Name = Cg.Player.Name;
//            store.Level = Cg.Player.Level;
//            foreach (var item in Cg.Items.All)
//            {
//                var list=Cg.Items.All.GroupBy(i => i.Name).ToList();

//            }



//        }

//        public bool IsOpeningBank => false;

//    }
//}
