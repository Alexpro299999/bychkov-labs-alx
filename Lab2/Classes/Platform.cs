namespace Lab2.Classes
{
    // 1. Использование собственных классов
    public class Platform
    {
        public int Number { get; set; }
        public bool IsCovered { get; set; }

        public Platform(int number, bool isCovered)
        {
            Number = number;
            IsCovered = isCovered;
        }

        public override string ToString()
        {
            return $"Platform #{Number} (Covered: {IsCovered})";
        }
    }
}