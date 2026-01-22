using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandyShop.DataAccess.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }

        [Required]
        public int CandyId { get; set; }
        [ForeignKey("CandyId")]
        [ValidateNever]
        public Candy Candy { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
    }
}