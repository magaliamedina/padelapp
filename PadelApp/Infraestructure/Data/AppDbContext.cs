using Microsoft.EntityFrameworkCore;
using PadelApp.Domain.Entities;

namespace PadelApp.Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<Booking> Bookings => Set<Booking>();
    }
}
