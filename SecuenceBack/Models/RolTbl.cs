using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
	public class RolTbl
	{
		[Key]
		[JsonIgnore]
		public int RolID { get; set; }
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
