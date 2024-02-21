using clothshop2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Reflection.Metadata;
namespace clothshop2.Models
{
    public class appDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=sql.bsite.net\MSSQL2016;Database=aharedy_;User Id =aharedy_; password=portsaid");
        }

        

       public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<log> Log { get; set; }

        public DbSet<theme> Theme { get; set; }

        public DbSet<operations> operations { get; set; }

        public DbSet<Question> questions { get; set; }


    }
}
