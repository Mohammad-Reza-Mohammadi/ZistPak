using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Cargo.CargoStatus
{
    public enum Status
    {
        Imported   = 1,  //وارد شده
        Suspended  = 2,  //معلق
        Confirm    = 3,  //تایید
        Rejected   = 4,  //رد شده
    }
}
