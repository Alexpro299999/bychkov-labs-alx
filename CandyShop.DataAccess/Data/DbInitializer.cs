using CandyShop.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CandyShop.DataAccess.Data
{
    public class DbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@candyshop.com",
                    Email = "admin@candyshop.com",
                    Name = "Admin User",
                    PhoneNumber = "1112223333",
                    StreetAddress = "Test 123 Ave",
                    State = "AdminState",
                    PostalCode = "23422",
                    City = "AdminCity"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@candyshop.com");
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }
            return;
        }
    }
}