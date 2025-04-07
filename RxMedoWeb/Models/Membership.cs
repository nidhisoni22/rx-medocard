using System.ComponentModel.DataAnnotations;

namespace RxMedoWeb.Models
{
    public class Membership
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string MembershipType { get; set; } = string.Empty;

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        [StringLength(20)]
        public string? MembershipNumber { get; set; }
    }
}
