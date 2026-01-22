using CandyShop.DataAccess.Models;

namespace CandyShop.Web.Models
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}