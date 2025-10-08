using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incro_IR.IRInstructions
{
    public class IRLoadConst : IRInst
    {
        public string Target;
        public long Value;
    }
}
