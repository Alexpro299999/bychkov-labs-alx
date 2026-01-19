using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public static class JsonProcessor
    {
        public static Dictionary<string, List<string>> GroupCarsByModel(List<Car> cars)
        {
            var groupedCars = new Dictionary<string, List<string>>();

            var groups = cars.GroupBy(c => c.ModelCar);

            foreach (var group in groups)
            {
                var lines = group.Select(c => $"{c.NameOwner}, {c.NumberCar}").ToList();
                groupedCars[group.Key] = lines;
            }

            return groupedCars;
        }
    }
}