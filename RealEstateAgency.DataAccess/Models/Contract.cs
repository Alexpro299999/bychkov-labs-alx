using System;

namespace RealEstateAgency.DataAccess.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractDate { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }

        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }

        public string EmployeeName { get; set; }
        public string ServiceName { get; set; }
        public decimal ServiceCost { get; set; }
    }
}