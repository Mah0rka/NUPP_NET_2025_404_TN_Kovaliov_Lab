namespace Fish.Common
{
    public class Aquarium
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Volume { get; set; } // Об'єм в літрах
        public string Location { get; set; }

        // Конструктор
        public Aquarium(string name, double volume, string location)
        {
            Id = Guid.NewGuid();
            Name = name;
            Volume = volume;
            Location = location;
        }

        // Метод
        public void DisplayInfo()
        {
            Console.WriteLine($"Акваріум '{Name}' (ID: {Id})");
            Console.WriteLine($"Об'єм: {Volume} літрів, Розташування: {Location}");
        }
    }
}

