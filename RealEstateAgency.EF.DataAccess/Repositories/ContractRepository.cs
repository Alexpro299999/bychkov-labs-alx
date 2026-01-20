using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RealEstateAgency.EF.DataAccess.Data;
using RealEstateAgency.EF.DataAccess.Models;

namespace RealEstateAgency.EF.DataAccess.Repositories
{
    public class ContractRepository
    {
        private readonly RealEstateDbContext _context;

        public ContractRepository(RealEstateDbContext context)
        {
            _context = context;
        }

        public List<Contract> GetAll()
        {
            return _context.Contracts
                .Include(c => c.Employee)
                .Include(c => c.Service)
                .ToList();
        }

        public void Add(Contract entity)
        {
            _context.Contracts.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Contract entity)
        {
            _context.Contracts.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Contracts.Find(id);
            if (entity != null)
            {
                _context.Contracts.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Contract> GetByEmployee(int employeeId)
        {
            return _context.Contracts
                .Include(c => c.Employee)
                .Include(c => c.Service)
                .Where(c => c.EmployeeId == employeeId)
                .ToList();
        }

        public IEnumerable GetEmployeeStats()
        {
            return _context.Employees
                .Select(e => new
                {
                    FullName = e.FullName,
                    ServicesCount = e.Contracts.Count,
                    TotalProfit = e.Contracts.Sum(c => c.Service.Cost)
                })
                .ToList();
        }

        public IEnumerable GetServiceDateStats()
        {
            return _context.Contracts
                .GroupBy(c => new { c.Service.Name, c.ContractDate })
                .Select(g => new
                {
                    ServiceName = g.Key.Name,
                    Date = g.Key.ContractDate,
                    Count = g.Count()
                })
                .ToList();
        }
    }
}