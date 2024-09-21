using GymManagment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace GymManagment.Connection
{

    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
           : base(options)
        {
        }
        
        

    }
}
