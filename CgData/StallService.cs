using System;
using CgData.Models;


namespace CgData
{
    public class StallService
    {
        private readonly CgContext _cgContext=new CgContext();


        public void AddStall(Stall newStall)
        {
            _cgContext.Stalls.Add(newStall);
            _cgContext.SaveChanges();
        }

        public void InsertOrUpdateStall(Stall stall)
        {
            var t0 = DateTime.Now;
            var s = _cgContext.Stalls.Find(stall.X,stall.Y);
            if (s != null)
            {
                _cgContext.Stalls.Remove(s);
            }
            _cgContext.Stalls.Add(stall);
            _cgContext.SaveChanges();
            Console.WriteLine((DateTime.Now.Ticks-t0.Ticks)/10000);
        }

        public void UpdateStore()
        {

        }
    }
}
