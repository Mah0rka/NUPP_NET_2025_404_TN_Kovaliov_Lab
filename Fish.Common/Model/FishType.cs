using System.Globalization;

namespace Fish.Common
{
    public struct FishType(string variety, string habitat, uint topspeed, bool ispredatory, double length)
    {
        public string Variety { get; private set; } = variety;
        public string Habitat { get; private set; } = habitat;
        public uint TopSpeed { get; private set; } = topspeed;
        public bool IsPredatory { get; private set; } = ispredatory;
        public double Length { get; private set; } = length; // Довжина риби
    }
}