using System.Collections.Generic;
using System.Linq;
using RealEstateAgency.EF.DataAccess.Data;
using RealEstateAgency.EF.DataAccess.Models;

namespace RealEstateAgency.EF.DataAccess.Repositories
{
    public class ServiceRepository
    {
        private readonly RealEstateDbContext _context;

        public ServiceRepository(RealEstateDbContext context)
        {
            _context = context;
        }

        public List<Service> GetAll()
        {
            return _context.Services.ToList();
        }

        public void Add(Service entity)
        {
            _context.Services.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Service entity)
        {
            _context.Services.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Services.Find(id);
            if (entity != null)
            {
                _context.Services.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Service> GetPopularServices()
        {
            return _context.Contracts
                .GroupBy(c => c.Service)
                .Where(g => g.Count() > 2)
                .Select(g => g.Key)
                .ToList();
        }
    }
}