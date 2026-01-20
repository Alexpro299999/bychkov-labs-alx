using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAgency.DataAccess.Models;

namespace RealEstateAgency.DataAccess.Repositories
{
    public class ContractRepository
    {
        public List<Contract> GetAll()
        {
            var list = new List<Contract>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT c.*, e.FullName as EmployeeName, s.Name as ServiceName, s.Cost as ServiceCost
                    FROM Contracts c
                    JOIN Employees e ON c.EmployeeId = e.Id
                    JOIN Services s ON c.ServiceId = s.Id";

                var cmd = new SqlCommand(sql, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapContract(reader));
                    }
                }
            }
            return list;
        }

        public List<Contract> GetByEmployee(int employeeId)
        {
            var list = new List<Contract>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetContractsByEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapContract(reader));
                    }
                }
            }
            return list;
        }

        public DataTable GetEmployeeStats()
        {
            var dt = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetEmployeeStats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetServiceDateStats()
        {
            var dt = new DataTable();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetServiceDatesStats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void Add(Contract contract)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Contracts (ContractNumber, ContractDate, ClientName, ClientPhone, EmployeeId, ServiceId) VALUES (@Num, @Date, @Client, @Phone, @EmpId, @ServId)", conn);
                cmd.Parameters.AddWithValue("@Num", contract.ContractNumber);
                cmd.Parameters.AddWithValue("@Date", contract.ContractDate);
                cmd.Parameters.AddWithValue("@Client", contract.ClientName);
                cmd.Parameters.AddWithValue("@Phone", contract.ClientPhone);
                cmd.Parameters.AddWithValue("@EmpId", contract.EmployeeId);
                cmd.Parameters.AddWithValue("@ServId", contract.ServiceId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Contract contract)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Contracts SET ContractNumber=@Num, ContractDate=@Date, ClientName=@Client, ClientPhone=@Phone, EmployeeId=@EmpId, ServiceId=@ServId WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", contract.Id);
                cmd.Parameters.AddWithValue("@Num", contract.ContractNumber);
                cmd.Parameters.AddWithValue("@Date", contract.ContractDate);
                cmd.Parameters.AddWithValue("@Client", contract.ClientName);
                cmd.Parameters.AddWithValue("@Phone", contract.ClientPhone);
                cmd.Parameters.AddWithValue("@EmpId", contract.EmployeeId);
                cmd.Parameters.AddWithValue("@ServId", contract.ServiceId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Contracts WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private Contract MapContract(SqlDataReader reader)
        {
            return new Contract
            {
                Id = (int)reader["Id"],
                ContractNumber = (string)reader["ContractNumber"],
                ContractDate = (DateTime)reader["ContractDate"],
                ClientName = (string)reader["ClientName"],
                ClientPhone = (string)reader["ClientPhone"],
                EmployeeId = (int)reader["EmployeeId"],
                ServiceId = (int)reader["ServiceId"],
                EmployeeName = reader["EmployeeName"].ToString(),
                ServiceName = reader["ServiceName"].ToString(),
                ServiceCost = (decimal)reader["ServiceCost"]
            };
        }
    }
}