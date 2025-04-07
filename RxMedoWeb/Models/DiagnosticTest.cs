using System.ComponentModel.DataAnnotations;

namespace RxMedoWeb.Models
{
    public class DiagnosticTest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal OriginalPrice { get; set; }

        [Range(0, 100000)]
        public decimal DiscountedPrice { get; set; }

        [Range(0, 100)]
        public int DiscountPercentage { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
