using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP5_TRPG
{
    class Item
    {
        public string Name { get; set; }
        public ItemStatus Status { get; set; }
        public int Value { get; set; }
        public int Gold { get; set; }
        public string Descript { get; set; }
        public Item(string name, ItemStatus status, int value, int gold, string descript)
        {
            Name = name;
            Status = status;
            Value = value;
            Gold = gold;
            Descript = descript;
        }
    }
}
