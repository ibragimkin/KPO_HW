using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class Wolf : Predator
    {
        public Wolf(int food, int kindness) : base(food, kindness) { }

        public override string ToString()
        {
            return "Wolf";
        }
    }
}
