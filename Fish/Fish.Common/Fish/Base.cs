using System.Xml.Linq;

namespace Fish.Common
{
    public abstract class FishBase
    {
        public FishType FishType { get; }
        public Guid Id { get; }

        // �������� ����
        public static int TotalFishCount;

        // ��������� �����������
        static FishBase()
        {
            TotalFishCount = 0;
        }

        // �����������
        public FishBase(FishType fishType)
        {
            FishType = fishType;
            Id = Guid.NewGuid();
            TotalFishCount++;
        }

        // ������� �� ����
        public delegate void FishActionHandler(FishBase fish);
        public event FishActionHandler? OnSwim;

        // Метод для виклику події (protected для нащадків)
        protected void RaiseOnSwim()
        {
            OnSwim?.Invoke(this);
        }

        public virtual void Swim()
        {
            Console.WriteLine($"{FishType.Variety} is swimming...");
            RaiseOnSwim();
        }

        // ��������� �����
        public static void PrintTotalFish()
        {
            Console.WriteLine($"Total fish created: {TotalFishCount}");
        }
    }
}