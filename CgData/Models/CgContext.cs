using System.Data.Entity;

namespace CgData.Models
{
    public class CgContext:DbContext
    {
        public CgContext() : base("Data Source=221.203.22.11;Initial Catalog=CrossGate;User ID=hyy;Password=Asd-19890811;Integrated Security=false")
        {

        }

        public DbSet<Stall> Stalls { get; set; }
        public DbSet<StallItem> StallItems { get; set; }
        public DbSet<Store> Stores { get; set; }
        
    }
}
