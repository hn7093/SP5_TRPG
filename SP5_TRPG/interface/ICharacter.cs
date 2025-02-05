using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP5_TRPG
{
    interface ICharacter
    {
        string Name { get; set; }
        int Level { get; set; }
        int Health { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }
        bool isDead { get; set; }
        int Gold { get; set; }
        bool TakeDamage(int damage);
    }
}
