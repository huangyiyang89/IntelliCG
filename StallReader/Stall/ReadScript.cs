//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using CgData;
//using IntelliCG.Script;

//namespace IntelliCG.Stall
//{
//    public class ReadScript:ThreadBase
//    {
        
//        public ReadScript(CrossGate cg) : base(cg)
//        {
//            Ranges=new List<ReadRange>();
//            HasReadStalls=new List<CgData.Models.Stall>();
//            StallService=new StallService();
//        }
      

//        public List<ReadRange> Ranges { get; set; }

//        private StallService StallService { get; set; }
//        public List<CgData.Models.Stall> HasReadStalls { get; set; }

//        protected override void ThreadFunction()
//        {
//            HasReadStalls.Clear();
//            foreach (var readRange in Ranges)
//            {
//                Cg.MoveTo(readRange.X,readRange.Y);
//                Cg.WaitForMoving();
//                var toReadStalls = Cg.StallReader.ReadNearbyStalls();
//                toReadStalls.RemoveAll(s => s.X > readRange.X + readRange.Range || s.Y > readRange.Y + readRange.Range);
//                if (readRange.Direction == RangeDirection.E2W)
//                {
//                    toReadStalls=toReadStalls.OrderByDescending(s => s.X).ThenBy(s=>s.Y).ToList();
//                }
//                if (readRange.Direction == RangeDirection.N2S)
//                {
//                    toReadStalls = toReadStalls.OrderBy(s => s.Y).ThenBy(s=>s.X).ToList();
//                }
//                if (readRange.Direction == RangeDirection.S2N)
//                {
//                    toReadStalls=toReadStalls.OrderByDescending(s => s.Y).ThenBy(s=>s.X).ToList();
//                }
//                if (readRange.Direction == RangeDirection.W2E)
//                {
//                    toReadStalls=toReadStalls.OrderBy(s => s.X).ThenBy(s=>s.Y).ToList();
//                }
//                foreach (var readStall in toReadStalls)
//                {
//                    if (HasReadStalls.Exists(s => s.X == readStall.X && s.Y == readStall.Y))
//                    {
//                        continue;
//                    }
//                    Cg.MoveTo(readStall.X,readStall.Y);
//                    Cg.WaitForMoving();
//                    System.Threading.Thread.Sleep(1000);
//                    Cg.RightClick(readStall.X,readStall.Y);
//                    System.Threading.Thread.Sleep(3000);
//                    Cg.StallReader.ReadStallItems(readStall);
//                    HasReadStalls.Add(readStall);
//                }
//            }

//            foreach (var hasReadStall in HasReadStalls)
//            {
//                StallService.InsertOrUpdateStall(hasReadStall);
//            }
            
//        }


//        public static ReadScript GetInstance(CrossGate cg)
//        {
//            var readScript =new ReadScript(cg);
//            readScript.Ranges.Add(new ReadRange(238,87,RangeDirection.E2W));
//            return readScript;
//        }
//    }
//}
