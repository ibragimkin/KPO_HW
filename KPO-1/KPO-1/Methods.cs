using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo;

namespace KPO_1
{
    public class Methods
    {
        public static Item GetItem()
        {
            int itemType = Menu.ConsoleMenu("Выберите вид животного", ZooClass.s_ItemsList);
            Console.WriteLine("Введите номер предмета:");
            int num = GetInt();
            Item item = null;
            switch (itemType)
            {
                case 0:
                    item = new Thing(num);
                    break;
                case 1:
                    item = new Table(num);
                    break;
                case 2:
                    item = new Computer(num);
                    break;
            }
            return item;
        }

        public static Animal GetAnimal()
        {
            int animalType = Menu.ConsoleMenu("Выберите вид животного", ZooClass.s_AnimalsList);
            Console.WriteLine("Введите потребляемое кол-во еды:");
            int foodCount = GetInt();
            Console.WriteLine("Введите уровень доброты:");
            int kindness = GetInt(0, 10);
            Animal animal = null;
            switch (animalType)
            {
                case 0:
                    animal = new Monkey(foodCount, kindness);
                    break;
                case 1:
                    animal = new Rabbit(foodCount, kindness);
                    break;
                case 2:
                    animal = new Tiger(foodCount, kindness);
                    break;
                case 3:
                    animal = new Wolf(foodCount, kindness);
                    break;
            }
            return animal;
            
        }

        

        /// <summary>
        /// Получает целое число от a до b от пользователя.
        /// </summary>
        /// <param name="a">Нижняя граница</param>
        /// <param name="b">Верхняя граница</param>
        /// <returns>Целое число от a до b</returns>
        public static int GetInt(int a = 0, int b = 10000)
        {
            int result;
            while (true)
            {
                try
                {
                    result = int.Parse(Console.ReadLine()!);
                    if (a <= result && result <= b)
                    {
                        return result;
                    }

                }
                catch { }
                Console.WriteLine($"Неправильно введены данные, введите целое число от {a} {(b == 10000 ? "." : $"до {b} ")}");

                }
            }
    }
}
