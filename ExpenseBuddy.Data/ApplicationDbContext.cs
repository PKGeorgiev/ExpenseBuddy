﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseBuddy.Data.Models;

namespace ExpenseBuddy.Data
{
    public class ExpenseBuddyDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseElement> ExpenseElements { get; set; }
        public DbSet<ExpensePayer> Payers { get; set; }

        public ExpenseBuddyDbContext(DbContextOptions<ExpenseBuddyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder
                .Entity<Bank>()
                .HasMany(b => b.BankAccounts)
                .WithOne(ba => ba.Bank)
                .HasForeignKey(fk => fk.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<BankAccount>()
                .HasOne(ba => ba.User)
                .WithMany(u => u.BankAccounts)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BankAccount>()
                .Property(t => t.IsActive)
                .HasDefaultValue(true);


            builder
                .Entity<Expense>()
                .HasOne(m => m.Element)
                .WithMany(m => m.Expenses)
                .HasForeignKey(m => m.ElementId);

            builder
                .Entity<Expense>()
                .HasOne(m => m.Owner)
                .WithMany(m => m.OwnedExpenses)
                .HasForeignKey(m => m.OwnerId);

            // Define PK
            builder
                .Entity<ExpensePayer>()
                .HasKey(m => new { m.ExpenseId, m.PayerId });

            builder
                .Entity<ExpensePayer>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 3)");

            builder
                .Entity<Expense>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 3)");

            builder
                .Entity<Expense>()
                .Property(p => p.Fee)
                .HasColumnType("decimal(18, 3)");

            builder
                .Entity<BankAccount>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 3)");


            // Define Many-To-Many Expense <=> ApplicationUser
            builder
                .Entity<Expense>()
                .HasMany(m => m.Payers)
                .WithOne(m => m.Expense)
                .HasForeignKey(m => m.ExpenseId);

            builder
                .Entity<ApplicationUser>()
                .HasMany(m => m.Expenses)
                .WithOne(m => m.Payer)
                .HasForeignKey(m => m.PayerId);
            
            


        }
    }
}
