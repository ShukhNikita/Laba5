using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseWorkCompany.Models;

namespace CourseWorkCompany.Data
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProductionType> ProductionTypes { get; set; }
        public DbSet<ProductReleasePlan> ProductReleasePlans { get; set; }
        public DbSet<CourseWorkCompany.Models.Product> Products { get; set; }
        public DbSet<ProductSalesPlan> ProductSalesPlans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ProductAnalysis;Integrated Security=True;Trust Server Certificate=True", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }

    }
}
