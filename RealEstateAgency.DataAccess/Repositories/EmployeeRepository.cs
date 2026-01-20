using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAgency.DataAccess.Models;

namespace RealEstateAgency.DataAccess.Repositories
{
    public class EmployeeRepository
    {
        public List<Employee> GetAll()
        {
            var list = new List<Employee>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Employees", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Employee
                        {
                            Id = (int)reader["Id"],
                            FullName = (string)reader["FullName"],
                            Position = (string)reader["Position"],
                            Address = (string)reader["Address"],
                            Phone = (string)reader["Phone"],
                            Education = (string)reader["Education"],
                            Specialty = (string)reader["Specialty"]
                        });
                    }
                }
            }
            return list;
        }

        public List<Employee> GetWithHigherEducation()
        {
            var list = new List<Employee>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetEmployeesWithHigherEducation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Employee
                        {
                            Id = (int)reader["Id"],
                            FullName = (string)reader["FullName"],
                            Position = (string)reader["Position"],
                            Address = (string)reader["Address"],
                            Phone = (string)reader["Phone"],
                            Education = (string)reader["Education"],
                            Specialty = (string)reader["Specialty"]
                        });
                    }
                }
            }
            return list;
        }

        public void Add(Employee emp)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Employees (FullName, Position, Address, Phone, Education, Specialty) VALUES (@Name, @Pos, @Addr, @Phone, @Edu, @Spec)", conn);
                cmd.Parameters.AddWithValue("@Name", emp.FullName);
                cmd.Parameters.AddWithValue("@Pos", emp.Position);
                cmd.Parameters.AddWithValue("@Addr", emp.Address);
                cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                cmd.Parameters.AddWithValue("@Edu", emp.Education);
                cmd.Parameters.AddWithValue("@Spec", emp.Specialty);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Employee emp)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Employees SET FullName=@Name, Position=@Pos, Address=@Addr, Phone=@Phone, Education=@Edu, Specialty=@Spec WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", emp.Id);
                cmd.Parameters.AddWithValue("@Name", emp.FullName);
                cmd.Parameters.AddWithValue("@Pos", emp.Position);
                cmd.Parameters.AddWithValue("@Addr", emp.Address);
                cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                cmd.Parameters.AddWithValue("@Edu", emp.Education);
                cmd.Parameters.AddWithValue("@Spec", emp.Specialty);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Employees WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}