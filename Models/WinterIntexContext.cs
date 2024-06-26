﻿using Microsoft.EntityFrameworkCore;


// Context file for the app

namespace WinterIntex.Models
{
    public class WinterIntexContext : DbContext
    {
        public WinterIntexContext(DbContextOptions<WinterIntexContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryProductOrder> CategoryProductOrders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemRecommendations> ItemRecommendations { get; set; }
        
        public DbSet<UserRecommendations> UserRecommendations { get; set; }
        public DbSet<TopRecommendations> TopRecommendations { get; set; }


        public DbSet<Order> Order => Set<Order>();
        public DbSet<EntryMethods> EntryMethods { get; set; }
        public DbSet<TransactionTypes> TransactionTypes { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CardTypes> CardTypes { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Color> Color { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the CategoryProductOrder entity to the category_product_order table
            modelBuilder.Entity<CategoryProductOrder>().ToTable("category_product_order");
            modelBuilder.Entity<Category>().ToTable("category");

        }

    }
}
