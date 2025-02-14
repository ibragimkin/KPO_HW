
namespace Zoo
{
    public class VeterinaryClinic : IVeterinaryClinic
    {
        public bool CheckHealth(Animal animal)
        {
            return new Random().Next(0, 10) != 0; // Случайный выбор для примера (10% шанс отклонения)
        }
    }
}
