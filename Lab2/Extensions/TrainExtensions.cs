using Lab2.Classes;

namespace Lab2.Extensions
{
    // 15. Использование методов расширения
    public static class TrainExtensions
    {
        public static bool IsHighSpeed(this Train train)
        {
            return train.Speed > 150;
        }
    }
}