namespace SecuenceBack.Models
{
    public class RolPermissionsRel
    {
        public int RolPermissionsRelID { get; set; }
        public int RolID { get; set; }
        public int PermissionsID {  get; set; }
        public int Status {  get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? DeletedAt { get; set; }
    }
}
