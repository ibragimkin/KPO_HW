namespace Zoo
{
    public class ZooClass
    {
        public static readonly string[] s_AnimalsList = { "Monkey", "Rabbit", "Tiger", "Wolf" };
        public static readonly string[] s_ItemsList = { "Thing", "Table", "Computer" };
        private readonly List<Animal> animals;
        private readonly List<Item> items;
        private IVeterinaryClinic vetClinic;
        public int FoodConsumption => animals.Sum(a => a.Food);
        public int AnimalsAmount => animals.Count;

        public ZooClass(IVeterinaryClinic clinic)
        {
            vetClinic = clinic;
            animals = new List<Animal>();
            items = new List<Item>();
        }

        /// <summary>
        /// Добавляет животное в список, с проверкой в клинике.
        /// </summary>
        /// <param name="animal">Животное для добавления.</param>
        public void AddAnimal(Animal animal)
        { 
            if (vetClinic.CheckHealth(animal)) animals.Add(animal);
            else
            {
                Console.WriteLine("Проверка ветклиники не пройдена.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Добавляет предмет в список.
        /// </summary>
        /// <param name="item">Предмет для добавления.</param>
        public void AddItem(Item item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Получает массив безопасных животных.
        /// </summary>
        /// <returns>Массив безопасных животных.</returns>
        public string[] GetSafeAnimals()
        {
            return animals.Where(a => a.Kindness>5).Select(a => $"{a.GetAnimalType()} | Food: {a.Food} | Kindness: {a.Kindness}").ToArray();
        }

        /// <summary>
        /// Удаляет животное по индексу.
        /// </summary>
        /// <param name="index">Индекс для удаления.</param>
        public void RemoveAnimal(int index)
        {
            animals.RemoveAt(index);
        }

        /// <summary>
        /// Удаляет предиет по индексу.
        /// </summary>
        /// <param name="index">Индекс для удаления.</param>
        public void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        /// <summary>
        /// Полный список животных со всеми данными.
        /// </summary>
        /// <returns></returns>
        public string[] GetFullAnimalsData()
        {
            
            List<string> data = new List<string> { };
            for (int i = 0; i < animals.Count; i++)
            {
                data.Add(animals[i].GetAnimalType() + " " + animals[i].ToString() + " | Food: " + animals[i].Food + " | Kindness: " + animals[i].Kindness);
            }
            return data.ToArray();
        }

        /// <summary>
        /// Полный список предметов со всеми данными.
        /// </summary>
        /// <returns></returns>
        public string[] GetFullItemsData()
        {
            List<string> data = new List<string> { };
            for (int i = 0; i < items.Count; i++)
            {
                data.Add(items[i].GetType().Name + " " + items[i].Number);
            }
            return data.ToArray();
        }

        public IReadOnlyList<Animal> GetAnimals() => animals.AsReadOnly();
        public IReadOnlyList<Item> GetItems() => items.AsReadOnly();

    }
}
