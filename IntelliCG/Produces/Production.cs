using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.Items;

namespace IntelliCG.Produces
{

    public class Production
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }

        public Dictionary<string, int> Raws;
    }

    public enum ProductionId
    {
        MianBao=900,FanQieJiang=909
    }
}
