using CandyShop.DataAccess.Models;

namespace CandyShop.Web.Models
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public double GrandTotal { get; set; }
    }
}
