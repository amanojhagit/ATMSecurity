using ATMDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMDemo.Data
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureAccounts(this ModelBuilder modelBuilder)
        {
            //Transacation Amount set to decimal 
            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionAmount)
                .HasColumnType("decimal(18,2)");

            //Account Amount set to decimal 
            modelBuilder.Entity<Account>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            //Account - AccountNo - unique
            modelBuilder.Entity<Account>()
                .HasAlternateKey(a => a.AccountNo)
                .HasName("AccountNo_Unique");


            //Admin - UserName - unique
            modelBuilder.Entity<Admin>()
                    .HasAlternateKey(a => a.UserName)
                    .HasName("UserName_Unique");

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountNo)
                .ValueGeneratedOnAdd();
        }
    }
}
