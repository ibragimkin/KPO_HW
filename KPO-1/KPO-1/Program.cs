using Zoo;
namespace KPO_1
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Welcome();
            IVeterinaryClinic clinic = new VeterinaryClinic();
            ZooClass zoo = new ZooClass(clinic);
            while (true)
            {
                int action = Menu.ConsoleMenu("Выберите действие", new string[] { "1. Добавить животное.", "2. Добавить предмет.", "3. Убрать животное", "4. Убрать предмет.", "5. Общий расход еды.", "6. Список животных, разрешённых в контактном зоопарке.", "7. Выход" });
                switch (action)
                {
                    case 0:
                        zoo.AddAnimal(Methods.GetAnimal());
                        break;
                    case 1:
                        zoo.AddItem(Methods.GetItem());
                        break;
                    case 2:
                        if (zoo.GetAnimals().Count == 0)
                        {
                            Console.WriteLine("Нет животных");
                            Console.ReadKey();
                            break;
                        }
                        zoo.RemoveAnimal(Menu.ConsoleMenu("Кого удалить?", zoo.GetFullAnimalsData()));
                        break;
                    case 3:
                        if (zoo.GetItems().Count == 0)
                        {
                            Console.WriteLine("Инвентарь пуст.");
                            Console.ReadKey();
                            break;
                        }
                        zoo.RemoveItem(Menu.ConsoleMenu("Что удалить?", zoo.GetFullItemsData()));
                        break;
                    case 4:
                        Console.WriteLine("Потребление на весь зоопарк:");
                        Console.WriteLine(zoo.FoodConsumption);
                        Console.ReadKey();
                        break;
                    case 5:
                        Menu.ConsoleMenu("Безопасные животные:", zoo.GetSafeAnimals());
                        break;
                    case 6:
                        return;

                }
            }
        }

        /// <summary>
        /// Выводит приветствие на экран.
        /// </summary>
        private static void Welcome()
        {
            Console.WriteLine("КПО ДЗ-1, Киндаров Ибрагим Муслимович, БПИ237");
            Greetings();
            Console.WriteLine("Для комфортной работы рекомендуется поставить окно консоли на весь экран.");
            Console.WriteLine("Управление меню: Стрелки/W+S - Вверх-вниз.   Enter/Space - Выбор");
            Console.ReadKey();
        }

        /// <summary>
        /// Приветствует пользователя в зависимости от времени суток.
        /// </summary>
        private static void Greetings()
        {
            int hour = (DateTime.Now).Hour;
            if (hour >= 4 && hour < 12) Console.WriteLine("Доброе утро!");
            else if (hour >= 12 && hour < 18) Console.WriteLine("Добрый день!");
            else if (hour >= 17 && hour < 22) Console.WriteLine("Добрый вечер!");
            else Console.WriteLine("Доброй ночи!");
        }
    }
}
