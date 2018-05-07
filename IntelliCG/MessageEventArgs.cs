using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG
{
    public class MessageEventArgs:EventArgs
    {
        public MessageEventArgs(string info)
        {
            Info = info;
        }
        public string Info { get; set; }
    }
}
