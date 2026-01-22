using System.ComponentModel.DataAnnotations;

namespace CandyShop.DataAccess.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Country { get; set; }
    }
}