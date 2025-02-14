using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public abstract class Animal : IAlive
    {
        public int Food { get; set; }
        public int Kindness { get; set; }

        protected Animal(int food, int kindness)
        {
            Food = food;
            Kindness = kindness;
        }
        public string GetAnimalType()
        {
            return "Animal";
        }
    }
}
