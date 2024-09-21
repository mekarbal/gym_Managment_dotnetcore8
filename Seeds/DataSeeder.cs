using GymManagment.Connection;
using GymManagment.Models.Enums;
using GymManagment.Models;
using Microsoft.AspNetCore.Identity;
using GymManagment.Identity;

namespace GymManagment.Seeds
{
    public class DataSeeder
    {
        private readonly ApplicationDBContext _context;

        public DataSeeder(ApplicationDBContext context)
        {
            _context = context;
        }

        public void SeedAdminUser()
        {

            if (!_context.Users.Any())
            {
                var passwordHelper = new PasswordHelper();

                var adminUser = new User
                {
                    Name = "Admin",
                    Email = "admin@admin.com",
                    Role = Role.ADMIN
                };

                adminUser.Password = passwordHelper.HashPassword("Admin123@");

                _context.Users.Add(adminUser);
                _context.SaveChanges();
            }
        }
    }

}
