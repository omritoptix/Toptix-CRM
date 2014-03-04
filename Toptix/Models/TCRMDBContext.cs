using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Toptix.Models
{

    public class TCRMDBContext : DbContext
    {


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Charge>()
            //     .HasRequired(e => e.Client);

            //modelBuilder.Entity<Charge>()
            //    .HasRequired(e => e.Order);

            //modelBuilder.Entity<Client>()
            //    .HasOptional(e => e.Country);

            //modelBuilder.Entity<Client>()
            //    .HasOptional(e => e.Distributor);

            //modelBuilder.Entity<Client>()
            //    .HasOptional(e => e.Currency);

            //modelBuilder.Entity<Client>()
            //    .HasOptional(e => e.PayType);

            //modelBuilder.Entity<Distributor>()
            //    .HasOptional(e => e.Country);

            //modelBuilder.Entity<Distributor>()
            //    .HasRequired(e => e.Country);

            //modelBuilder.Entity<Order>()
            //    .HasRequired(e => e.Currency);

            //modelBuilder.Entity<OrderItem>()
            //    .HasRequired(e => e.Product);

            //modelBuilder.Entity<OrderItem>().HasKey(e => e.OrderID).Property(e => e.OrderID);

            modelBuilder.Entity<OrderItem>()
                .HasRequired(o => o.Order)
                .WithMany(o => o.orderItem)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Charge>()
                .HasRequired(c => c.Order)
                .WithMany(o => o.Charge)
                .WillCascadeOnDelete();

            

            base.OnModelCreating(modelBuilder);

            

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Charge> Charges { get; set; }

        public DbSet<PayType> PayTypes { get; set; }

        public DbSet<PayCondition> PayConditions { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Distributor> Distributors { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<EnumCalculationType> EnumCalculationTypes { get; set; }

        public DbSet<EnumChargeFrequency> EnumChargeFrequencies { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<GlobalParameters> GlobalParameteres { get; set; }

        public DbSet<ConversionRate> ConversionRates { get; set; }



    }

   
}