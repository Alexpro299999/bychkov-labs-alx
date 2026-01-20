using System;
using System.Linq;
using RealEstateAgency.EF.DataAccess.Models;

namespace RealEstateAgency.EF.DataAccess.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RealEstateDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Employees.Any())
            {
                return;
            }

            var employees = new[]
            {
                new Employee { FullName = "Иванов Иван Иванович", Position = "Риелтор", Address = "ул. Ленина", Phone = "89001112233", Education = "Высшее", Specialty = "Менеджмент" },
                new Employee { FullName = "Петров Петр Петрович", Position = "Менеджер", Address = "ул. Мира", Phone = "89002223344", Education = "Среднее", Specialty = "Торговля" },
                new Employee { FullName = "Сидоров Сидор Сидорович", Position = "Агент", Address = "ул. Гагарина", Phone = "89003334455", Education = "Высшее", Specialty = "Юриспруденция" },
                new Employee { FullName = "Кузнецова Анна Павловна", Position = "Брокер", Address = "ул. Победы", Phone = "89004445566", Education = "Высшее", Specialty = "Экономика" },
                new Employee { FullName = "Смирнов Алексей Дмитриевич", Position = "Риелтор", Address = "ул. Кирова", Phone = "89005556677", Education = "Среднее", Specialty = "Строительство" }
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();

            var services = new[]
            {
                new Service { Name = "Покупка квартиры", Cost = 50000 },
                new Service { Name = "Продажа дома", Cost = 100000 },
                new Service { Name = "Аренда жилья", Cost = 15000 },
                new Service { Name = "Оценка недвижимости", Cost = 5000 },
                new Service { Name = "Юридическое сопровождение", Cost = 25000 }
            };
            context.Services.AddRange(services);
            context.SaveChanges();

            var r = new Random();
            var empList = context.Employees.ToList();
            var srvList = context.Services.ToList();

            for (int i = 1; i <= 50; i++)
            {
                var contract = new Contract
                {
                    ContractNumber = $"Д-{1000 + i}",
                    ContractDate = DateTime.Now.AddDays(-r.Next(0, 60)),
                    ClientName = $"Клиент_{i}",
                    ClientPhone = $"8900{r.Next(1000000, 9999999)}",
                    Employee = empList[r.Next(empList.Count)],
                    Service = srvList[r.Next(srvList.Count)]
                };
                context.Contracts.Add(contract);
            }
            context.SaveChanges();
        }
    }
}