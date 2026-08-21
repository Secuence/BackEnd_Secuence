using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecuenceBack.Models
{
    public class HealthCenterTbl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HealthCenterID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Direction { get; set; } = string.Empty;

        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string WebSite { get; set; } = string.Empty;

        public int Status { get; set; }
        public int MedicCount { get; set; }

        [Column(TypeName = "numeric(8,2)")]
        public decimal PatientsAvg { get; set; }

        [MaxLength(100)]
        public string? Response { get; set; }

        public int? PoliciesAccepted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? PoliciesAcceptedAt { get; set; }
    }
}