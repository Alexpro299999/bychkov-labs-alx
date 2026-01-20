using System.Data.SqlClient;

namespace RealEstateAgency.DataAccess
{
    public static class DbConnectionHelper
    {
        public static readonly string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=RealEstateAgencyDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}