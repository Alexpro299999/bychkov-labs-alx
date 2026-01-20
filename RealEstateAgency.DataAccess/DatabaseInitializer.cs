using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RealEstateAgency.DataAccess
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            string masterConnection = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";
            using (var connection = new SqlConnection(masterConnection))
            {
                connection.Open();
                var cmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RealEstateAgencyDB') CREATE DATABASE RealEstateAgencyDB", connection);
                cmd.ExecuteNonQuery();
            }

            using (var connection = DbConnectionHelper.GetConnection())
            {
                connection.Open();

                var createEmployeesTable = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
                    CREATE TABLE Employees (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        FullName NVARCHAR(100),
                        Position NVARCHAR(100),
                        Address NVARCHAR(200),
                        Phone NVARCHAR(50),
                        Education NVARCHAR(100),
                        Specialty NVARCHAR(100)
                    )";
                new SqlCommand(createEmployeesTable, connection).ExecuteNonQuery();

                var createServicesTable = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Services' AND xtype='U')
                    CREATE TABLE Services (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100),
                        Cost DECIMAL(18,2)
                    )";
                new SqlCommand(createServicesTable, connection).ExecuteNonQuery();

                var createContractsTable = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Contracts' AND xtype='U')
                    CREATE TABLE Contracts (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        ContractNumber NVARCHAR(50),
                        ContractDate DATETIME,
                        ClientName NVARCHAR(100),
                        ClientPhone NVARCHAR(50),
                        EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
                        ServiceId INT FOREIGN KEY REFERENCES Services(Id)
                    )";
                new SqlCommand(createContractsTable, connection).ExecuteNonQuery();

                CreateStoredProcedures(connection);
                SeedData(connection);
            }
        }

        private static void CreateStoredProcedures(SqlConnection connection)
        {
            ExecuteSql(connection, @"
                CREATE OR ALTER PROCEDURE sp_GetEmployeesWithHigherEducation
                AS
                BEGIN
                    SELECT * FROM Employees WHERE Education LIKE N'%высшее%'
                END");

            ExecuteSql(connection, @"
                CREATE OR ALTER PROCEDURE sp_GetPopularServices
                AS
                BEGIN
                    SELECT s.Id, s.Name, s.Cost 
                    FROM Services s
                    JOIN Contracts c ON s.Id = c.ServiceId
                    GROUP BY s.Id, s.Name, s.Cost
                    HAVING COUNT(c.Id) > 2
                END");

            ExecuteSql(connection, @"
                CREATE OR ALTER PROCEDURE sp_GetContractsByEmployee
                @EmployeeId INT
                AS
                BEGIN
                    SELECT c.*, e.FullName as EmployeeName, s.Name as ServiceName, s.Cost as ServiceCost
                    FROM Contracts c
                    JOIN Employees e ON c.EmployeeId = e.Id
                    JOIN Services s ON c.ServiceId = s.Id
                    WHERE c.EmployeeId = @EmployeeId
                END");

            ExecuteSql(connection, @"
                 CREATE OR ALTER PROCEDURE sp_GetEmployeeStats
                 AS
                 BEGIN
                    SELECT e.FullName, COUNT(c.Id) as ServicesCount, SUM(s.Cost) as TotalProfit
                    FROM Employees e
                    JOIN Contracts c ON e.Id = c.EmployeeId
                    JOIN Services s ON c.ServiceId = s.Id
                    WHERE DATEDIFF(month, c.ContractDate, GETDATE()) = 0
                    GROUP BY e.FullName
                 END");

            ExecuteSql(connection, @"
                 CREATE OR ALTER PROCEDURE sp_GetServiceDatesStats
                 AS
                 BEGIN
                    SELECT s.Name as ServiceName, c.ContractDate, COUNT(c.Id) as DailyCount
                    FROM Services s
                    JOIN Contracts c ON s.Id = c.ServiceId
                    GROUP BY s.Name, c.ContractDate
                 END");
        }

        private static void SeedData(SqlConnection connection)
        {
            int empCount = 0;
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", connection))
            {
                empCount = (int)cmd.ExecuteScalar();
            }

            if (empCount == 0)
            {
                var employees = new List<string>
                {
                    "INSERT INTO Employees VALUES (N'Иванов Иван Иванович', N'Риелтор', N'ул. Ленина, 10', N'89001112233', N'Высшее', N'Менеджмент')",
                    "INSERT INTO Employees VALUES (N'Петров Петр Петрович', N'Менеджер', N'ул. Мира, 5', N'89002223344', N'Среднее', N'Торговля')",
                    "INSERT INTO Employees VALUES (N'Сидоров Сидор Сидорович', N'Агент', N'ул. Гагарина, 12', N'89003334455', N'Высшее', N'Юриспруденция')",
                    "INSERT INTO Employees VALUES (N'Кузнецова Анна Павловна', N'Брокер', N'ул. Победы, 3', N'89004445566', N'Высшее', N'Экономика')",
                    "INSERT INTO Employees VALUES (N'Смирнов Алексей Дмитриевич', N'Риелтор', N'ул. Кирова, 8', N'89005556677', N'Среднее специальное', N'Строительство')",
                    "INSERT INTO Employees VALUES (N'Попов Дмитрий Сергеевич', N'Стажер', N'ул. Лесная, 1', N'89006667788', N'Неоконченное высшее', N'Маркетинг')",
                    "INSERT INTO Employees VALUES (N'Соколова Елена Викторовна', N'Менеджер', N'ул. Садовая, 15', N'89007778899', N'Высшее', N'Управление')",
                    "INSERT INTO Employees VALUES (N'Михайлов Михаил Михайлович', N'Агент', N'ул. Цветочная, 7', N'89008889900', N'Среднее', N'Общее')",
                    "INSERT INTO Employees VALUES (N'Новикова Ольга Игоревна', N'Риелтор', N'ул. Парковая, 22', N'89009990011', N'Высшее', N'Психология')",
                    "INSERT INTO Employees VALUES (N'Федоров Федор Федорович', N'Директор', N'ул. Центральная, 1', N'89000001122', N'Высшее', N'Бизнес')"
                };

                foreach (var sql in employees) ExecuteSql(connection, sql);

                var services = new List<string>
                {
                    "INSERT INTO Services VALUES (N'Покупка квартиры', 50000)",
                    "INSERT INTO Services VALUES (N'Продажа дома', 100000)",
                    "INSERT INTO Services VALUES (N'Аренда жилья', 15000)",
                    "INSERT INTO Services VALUES (N'Оценка недвижимости', 5000)",
                    "INSERT INTO Services VALUES (N'Юридическое сопровождение', 25000)",
                    "INSERT INTO Services VALUES (N'Ипотечный брокеридж', 20000)",
                    "INSERT INTO Services VALUES (N'Консультация', 3000)",
                    "INSERT INTO Services VALUES (N'Сдача в аренду', 12000)",
                    "INSERT INTO Services VALUES (N'Продажа участка', 40000)",
                    "INSERT INTO Services VALUES (N'Обмен недвижимости', 55000)"
                };

                foreach (var sql in services) ExecuteSql(connection, sql);

                var r = new Random();
                for (int i = 1; i <= 60; i++)
                {
                    int empId = r.Next(1, 11);
                    int srvId = r.Next(1, 11);
                    DateTime date = DateTime.Now.AddDays(-r.Next(0, 60));
                    string clientName = $"Клиент_{i}";
                    string phone = $"8900{r.Next(1000000, 9999999)}";
                    string num = $"Д-{1000 + i}";
                    string sql = $"INSERT INTO Contracts (ContractNumber, ContractDate, ClientName, ClientPhone, EmployeeId, ServiceId) VALUES (N'{num}', '{date:yyyy-MM-dd}', N'{clientName}', N'{phone}', {empId}, {srvId})";
                    ExecuteSql(connection, sql);
                }
            }
        }

        private static void ExecuteSql(SqlConnection connection, string sql)
        {
            using (var cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}