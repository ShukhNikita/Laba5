using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkCompany.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string FIO { get; set; }
        public string KindOfActivity { get; set; }
        public string TypeOwnership { get; set; }
        public ICollection<ProductReleasePlan> ProductReleasePlans { get; set; }
        public ICollection<ProductSalesPlan> ProductSalesPlans { get; set; }
        public Company()
        {
            ProductReleasePlans = new List<ProductReleasePlan>();
            ProductSalesPlans = new List<ProductSalesPlan>();
        }
    }
}
