using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public class Item : IInventory
    {
        public int Number {  get; set; }
        public Item(int num) { 
            Number = num;
        }
    }
}
