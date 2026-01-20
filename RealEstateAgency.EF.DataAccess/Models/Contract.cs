using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.EF.DataAccess.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; } = null!;
        public DateTime ContractDate { get; set; }
        public string ClientName { get; set; } = null!;
        public string ClientPhone { get; set; } = null!;

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; } = null!;
    }
}