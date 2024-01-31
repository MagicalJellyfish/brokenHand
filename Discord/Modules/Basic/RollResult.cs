using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brokenHand.Discord.Modules.Basic
{
    public class RollResult
    {
        public RollResult(int result, string detail)
        {
            Result = result;
            Detail = detail;
        }

        public int Result { get; set; }
        public string Detail { get; set; }
    }
}
