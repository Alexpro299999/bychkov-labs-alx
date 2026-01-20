using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RealEstateAgency.DataAccess.Models;

namespace RealEstateAgency.DataAccess.Repositories
{
    public class ServiceRepository
    {
        public List<Service> GetAll()
        {
            var list = new List<Service>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Services", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Service
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Cost = (decimal)reader["Cost"]
                        });
                    }
                }
            }
            return list;
        }

        public List<Service> GetPopularServices()
        {
            var list = new List<Service>();
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("sp_GetPopularServices", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Service
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Cost = (decimal)reader["Cost"]
                        });
                    }
                }
            }
            return list;
        }

        public void Add(Service service)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Services (Name, Cost) VALUES (@Name, @Cost)", conn);
                cmd.Parameters.AddWithValue("@Name", service.Name);
                cmd.Parameters.AddWithValue("@Cost", service.Cost);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Service service)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Services SET Name=@Name, Cost=@Cost WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", service.Id);
                cmd.Parameters.AddWithValue("@Name", service.Name);
                cmd.Parameters.AddWithValue("@Cost", service.Cost);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = DbConnectionHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Services WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}