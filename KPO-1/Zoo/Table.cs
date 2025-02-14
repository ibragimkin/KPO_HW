using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class Table : Item
    {
        public int Number { get; set; }
        public Table(int num) : base(num) { }
    }
}
