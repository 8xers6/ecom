using ECOM.Models;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserInformationViewModel> Users { get; set; }

        public DbSet<ProductViewModel> Product { get; set; }

        public DbSet<AddToCart> Atc { get; set; }
    }
}