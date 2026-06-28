using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
	public class PermissionsTbl
	{
		[Key]
		[JsonIgnore]
		public int PermissionsID { get; set; }
		public string Name { get; set; }

        public int Status { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? DeletedAt { get; set; }
    }
}
