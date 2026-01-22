using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CandyShop.DataAccess.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CandyId { get; set; }
        [ForeignKey("CandyId")]
        public Candy Candy { get; set; }

        public int Count { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}