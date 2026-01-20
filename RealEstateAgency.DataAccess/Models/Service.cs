namespace RealEstateAgency.DataAccess.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}