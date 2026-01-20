using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.EF.DataAccess.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public override string ToString()
        {
            return Name;
        }
    }
}