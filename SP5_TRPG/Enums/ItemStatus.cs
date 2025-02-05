using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP5_TRPG
{
    enum ItemStatus
    {
        [Description("공격력")]
        Attack,

        [Description("체력")]
        Health,

        [Description("방어력")]
        Defense
    }
}
