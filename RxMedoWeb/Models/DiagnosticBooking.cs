using System.ComponentModel.DataAnnotations;

namespace RxMedoWeb.Models
{
    public class DiagnosticBooking
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [Range(1, 120)]
        public int PatientAge { get; set; }

        [Required]
        [StringLength(10)]
        public string PatientGender { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TestType { get; set; } = string.Empty;

        [Required]
        public DateTime PreferredDate { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string ContactNumber { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public bool IsConfirmed { get; set; } = false;
    }
}
