using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class Computer : Item
    {
        public int Number { get; set; }
        public Computer(int num) : base(num) { }
    }
}
