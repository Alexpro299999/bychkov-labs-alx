using CandyShop.DataAccess.Models;

namespace CandyShop.DataAccess.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Category> Category { get; }
        IRepository<Brand> Brand { get; }
        IRepository<Candy> Candy { get; }
        IRepository<Order> Order { get; }
        IRepository<OrderDetail> OrderDetail { get; }
        IRepository<ApplicationUser> ApplicationUser { get; }

        Task SaveAsync();
    }
}