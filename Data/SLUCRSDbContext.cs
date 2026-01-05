using Microsoft.EntityFrameworkCore;
using System;
using UniversalRedemptionService.API.Models;

namespace UniversalRedemptionService.API.Data
{
    public class SLUCRSDbContext : DbContext
    {

        public SLUCRSDbContext(DbContextOptions<SLUCRSDbContext> options)
        : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<CashSendRedemption> CashSendRedemptions => Set<CashSendRedemption>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>().HasOne(u => u.Wallet).WithOne(w => w.User).HasForeignKey<Wallet>(w => w.UserId);

            // Wallet
            modelBuilder.Entity<Wallet>().Property(w => w.Balance).HasPrecision(18, 2);

            // Transactions
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>().HasOne(t => t.FromWallet).WithMany().HasForeignKey(t => t.FromWalletId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(t => t.ToWallet).WithMany().HasForeignKey(t => t.ToWalletId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasIndex(t => t.Reference).IsUnique();


            // Cash Send Redemptions
            modelBuilder.Entity<CashSendRedemption>().Property(r => r.Amount).HasPrecision(18, 2);
        }

    }
}
