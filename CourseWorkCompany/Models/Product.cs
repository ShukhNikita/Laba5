using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkCompany.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Characteristic { get; set; }

        [ForeignKey("ProductionType")]
        public int ProductionTypeId { get; set; }
        public ProductionType ProductionType { get; set; }
        public string MeasurementUnit { get; set; }
        public string? ProductPhotoPath { get; set; }
        public string? ProductFileNamePath { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

    }
}
