using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkCompany.Models
{
    public class ProductReleasePlan
    {
        [Key]
        public int ProductReleasePlanId { get; set; }
        public Company Company { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public ProductionType ProductionType { get; set; }
        [ForeignKey("ProductionType")]
        public int ProductionTypeId { get; set; }
        public double PlannedOutputVolume { get; set; }
        public double ActualOutputVolume { get; set; }
        public DateTime QuarterInfo { get; set; }
        public DateTime YearInfo { get; set; }
    }
}
