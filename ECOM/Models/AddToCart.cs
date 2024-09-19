using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOM.Models
{
    public class AddToCart
    {


        [Key]
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int product_id { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? product_image { get; set; }

        [Required]
        public string product_price { get; set; }
        [Required]
        public string product_name { get; set; }
        [Required]
        public string product_variant { get; set; }
        [Required]
        public string product_quantity { get; set; }
        [Required]
        public string fullname { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string contact { get; set; }
        [Required]
        public string email { get; set; }

        public string note { get; set; }
    }
}
