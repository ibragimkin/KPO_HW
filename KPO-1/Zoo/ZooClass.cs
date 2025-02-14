namespace Zoo
{
    public class ZooClass
    {
        public static readonly string[] s_AnimalsList = { "Monkey", "Rabbit", "Tiger", "Wolf" };
        public static readonly string[] s_ItemsList = { "Thing", "Table", "Computer" };
        private readonly List<Animal> animals;
        private readonly List<Item> items;
        public int FoodConsumption => animals.Sum(a => a.Food);
        public int AnimalsAmount => animals.Count;

        public ZooClass()
        {
            animals = new List<Animal>();
            items = new List<Item>();
        }

        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public string[] GetSafeAnimals()
        {
            return animals.Where(a => a.Kindness>5).Select(a => $"{a.GetAnimalType()} | Food: {a.Food} | Kindness: {a.Kindness}").ToArray();
        }

        public void RemoveAnimal(int index)
        {
            animals.RemoveAt(index);
        }
        public void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }
        public string[] GetFullAnimalsData()
        {
            
            List<string> data = new List<string> { };
            for (int i = 0; i < animals.Count; i++)
            {
                data.Add(animals[i].GetAnimalType() + " " + animals[i].ToString() + " | Food: " + animals[i].Food + " | Kindness: " + animals[i].Kindness);
            }
            return data.ToArray();
        }
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
