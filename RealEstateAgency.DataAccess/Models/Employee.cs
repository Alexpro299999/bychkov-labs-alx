namespace RealEstateAgency.DataAccess.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Education { get; set; }
        public string Specialty { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}