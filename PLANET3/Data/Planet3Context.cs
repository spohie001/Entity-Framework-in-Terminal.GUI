using PLANET3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Terminal.Gui;

namespace PLANET3.Data
{
    public class Planet3Context : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        // public DbSet<ECustomer> ECustomers { get; set; } = null!;
        // public DbSet<EOrder> EOrders { get; set; } = null!;
        public DbSet<Item> Item { get; set; } = null!;
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<ItemAmount> ItemAmount { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Aaa");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Items)
                .WithMany(e => e.Orders);
        }
        // public void Configure(ModelBuilder builder)
        // {
        //     builder.Entity<Order>()
        //         .HasMany(c => c.Items)
        //         .WithOne(e => e.Order);
        //     builder.Entity<Item>()
        //         .HasMany(c => c.ItemAmounts)
        //         .WithOne(e => e.Item);
        //
        // }
    }
}
