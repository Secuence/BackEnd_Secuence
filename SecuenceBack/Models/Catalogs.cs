using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
    public class Catalogs
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Catalog { get; set; }
    }
}
