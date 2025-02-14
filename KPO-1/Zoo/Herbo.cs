using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class Herbo : Animal
    {
        public Herbo(int food, int kindness) : base(food, kindness) { }

        /// <summary>
        /// Возвращает вид животного.
        /// </summary>
        /// <returns>Вид животного.</returns>
        public new string GetAnimalType()
        {
            return "Herbo";
        }
    }
}
