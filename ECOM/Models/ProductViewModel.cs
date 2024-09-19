using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOM.Models
{
    public class ProductViewModel
    {
        [Key]
        [Required]
        public int product_id { get; set; }
        [Required]
        public string product_owner { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? product_image { get; set; }

        [Required]
        public string product_name { get; set; }

        [Required]
        public string product_description { get; set; }

        [Required]
        public string product_stocks { get; set; }

        [Required]
        public string product_variant { get; set; }

        [Required]
        public double product_price { get; set; } 
    }

}
