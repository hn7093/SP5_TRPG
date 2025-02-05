using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP5_TRPG
{
    enum Job
    {
        [Description("전사")]
        Warrior = 1,
        [Description("도적")]
        Thief
    }
}
