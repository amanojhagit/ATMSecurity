using Microsoft.EntityFrameworkCore;
using ATMDemo.Models;

namespace ATMDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> User { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //calling COnfigureAccounts in ModelBuilderExtensions 
            modelBuilder.ConfigureAccounts();
        }
    }
}
