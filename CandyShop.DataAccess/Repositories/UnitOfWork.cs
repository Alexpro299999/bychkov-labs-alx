using CandyShop.DataAccess.Data;
using CandyShop.DataAccess.Models;

namespace CandyShop.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new Repository<Category>(_db);
            Brand = new Repository<Brand>(_db);
            Candy = new Repository<Candy>(_db);
            Order = new Repository<Order>(_db);
            OrderDetail = new Repository<OrderDetail>(_db);
            ApplicationUser = new Repository<ApplicationUser>(_db);
        }

        public IRepository<Category> Category { get; private set; }
        public IRepository<Brand> Brand { get; private set; }
        public IRepository<Candy> Candy { get; private set; }
        public IRepository<Order> Order { get; private set; }
        public IRepository<OrderDetail> OrderDetail { get; private set; }
        public IRepository<ApplicationUser> ApplicationUser { get; private set; }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}