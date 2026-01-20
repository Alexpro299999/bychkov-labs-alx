using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RealEstateAgency.EF.DataAccess.Data;
using RealEstateAgency.EF.DataAccess.Models;

namespace RealEstateAgency.EF.DataAccess.Repositories
{
    public class EmployeeRepository
    {
        private readonly RealEstateDbContext _context;

        public EmployeeRepository(RealEstateDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public Employee? GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        public void Add(Employee entity)
        {
            _context.Employees.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Employee entity)
        {
            _context.Employees.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Employees.Find(id);
            if (entity != null)
            {
                _context.Employees.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Employee> GetWithHigherEducation()
        {
            return _context.Employees
                .Where(e => e.Education.Contains("Высшее") || e.Education.Contains("высшее"))
                .ToList();
        }
    }
}