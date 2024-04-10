﻿using Microsoft.EntityFrameworkCore;

namespace WinterIntex.Models
{
    public class WinterIntexContext : DbContext
    {
        public WinterIntexContext(DbContextOptions<WinterIntexContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryProductOrder> CategoryProductOrders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemRecommendations> ItemRecommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the CategoryProductOrder entity to the category_product_order table
            modelBuilder.Entity<CategoryProductOrder>().ToTable("category_product_order");
            modelBuilder.Entity<Category>().ToTable("category");

        }

    }
}
