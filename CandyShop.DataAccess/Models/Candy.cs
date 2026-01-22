using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CandyShop.DataAccess.Models
{
    public class Candy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        [ValidateNever]
        public Brand Brand { get; set; }
    }
}