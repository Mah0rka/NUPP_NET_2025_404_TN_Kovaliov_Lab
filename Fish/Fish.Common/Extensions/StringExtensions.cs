namespace Fish.Common.Extensions
{
    // Метод розширення
    public static class StringExtensions
    {
        // Метод розширення для форматування назв риб
        public static string ToFishDisplayName(this string fishName)
        {
            if (string.IsNullOrWhiteSpace(fishName))
                return "Невідома риба";

            // Форматування назви риби
            return $"*** {fishName.ToUpper()} ***";
        }

        // Метод розширення
        public static string ToAquariumLabel(this string aquariumName)
        {
            if (string.IsNullOrWhiteSpace(aquariumName))
                return "Без назви";

            return $"=== Акваріум: {aquariumName} ===";
        }
    }
}

