using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAgency.EF.DataAccess.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Education { get; set; } = null!;
        public string Specialty { get; set; } = null!;

        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public override string ToString()
        {
            return FullName;
        }
    }
}