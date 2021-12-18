using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public enum TransferTarget : UInt16
    {
        Present,
        Salary,
        Bribe,
        Payment,
        Other
    }
}
