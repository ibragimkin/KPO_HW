using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class Predator : Animal
    {
        public Predator(int food, int kindness) : base(food, kindness) { }

        public override string ToString()
        {
            return "Predator";
        }
        public new string GetAnimalType()
        {
            return "Predator";
        }
    }
}
