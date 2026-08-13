using System.ComponentModel;

namespace SecuenceBack.Models
{
    public class MedicTbl
    {  
        public int MedicID { get; set; }
        public string? License { get; set; }
        public string? Specialization { get; set; }
        public string? Country { get; set; }
        public string? Degree { get; set; }
        public float PatientsAvg { get; set; }
        public float PhoneNumber {  get; set; }
        public string WebSite { get; set; }
        public string ? MedicTypeType { get;set; }
        public string ? Response { get; set; }
        public int Status { get; set; }
        public int PoliciesAccepted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteedAt { get; set; }
        public DateTime? PoliciesAcceptedAt { get; set; }
    }

    public class MedicDto
    {
        public string? Direction { get; set; }
        public string? License { get; set; }
        public string? Specialization { get; set; }
        public string? Country { get; set; }
        public string? Degree { get; set; }
        public float PatientsAvg { get; set; }
        public float PhoneNumber { get; set; }
        public string WebSite { get; set; }
        public string? MedicTypeType { get; set; }
        public string? Response { get; set; }
        public int Status { get; set; }
        public int PoliciesAccepted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteedAt { get; set; }
        public DateTime? PoliciesAcceptedAt { get; set; }
        public int RolID { get; set; }
    }
}
