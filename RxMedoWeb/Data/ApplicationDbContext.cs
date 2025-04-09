using Microsoft.EntityFrameworkCore;
using RxMedoWeb.Models;

namespace RxMedoWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Membership> Memberships { get; set; }
        public DbSet<DiagnosticBooking> DiagnosticBookings { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        // Add other DbSets for future models as needed
        // public DbSet<Hospital> Hospitals { get; set; }
        // public DbSet<Offer> Offers { get; set; }
    }
}
