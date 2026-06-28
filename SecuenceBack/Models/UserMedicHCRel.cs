namespace SecuenceBack.Models
{
    public class UserMedicHCRel
    {
        public int UserMedicHCRelID { get; set; }
        public int UserID { get; set; }
        public int MedicID { get; set; }
        public int HealthCenterID { get; set; }
        public string Direction { get; set; }
        public int RollID {  get; set; }
        public int Status { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? DeletedAt { get; set; }


    }
}
