using BankingConsoleApp;
using LibrairieProjetDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace WebAPI.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        //public DbSet<Client> Clients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration pour Client
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Account>().Property(a => a.Balance)
              .HasColumnType("decimal(18, 2)");
           modelBuilder.Entity<PrivateClient>().ToTable("ClientsPrive");
            modelBuilder.Entity<ProfessionnalClient>().ToTable("ClientsPro");
            modelBuilder.Entity<CarteBleue>().ToTable("CarteBleue");
            modelBuilder.Entity<Anomalies>().ToTable("Anomalies");
            modelBuilder.Entity<Anomalies>().Property(a => a.Amount)
           .HasColumnType("decimal(7, 2)");


            // Configuration pour Transaction
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // Précision totale de 18 chiffres, dont 2 après la virgule
            // Exemple de données seed
            modelBuilder.Entity<Account>().HasData(
                new Account { AccountId = 1, Balance = 5000, AccountType = "Pro" }
            );

           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}