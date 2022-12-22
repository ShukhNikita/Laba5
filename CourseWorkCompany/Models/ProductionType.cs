using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkCompany.Models
{
    public class ProductionType
    {
        [Key]
        public int ProductionTypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductReleasePlan> ProductReleasePlans { get; set; }
        public ICollection<ProductSalesPlan> ProductSalesPlans { get; set; }
        public ProductionType()
        {
            Products = new List<Product>();
            ProductReleasePlans = new List<ProductReleasePlan>();
            ProductSalesPlans = new List<ProductSalesPlan>();
        }
    }
}
