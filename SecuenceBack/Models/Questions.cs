using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
    public class Questions
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string questions { get; set; }
        public string? position { get; set; }
    }
    public class QuestionsByApplications
    {
        [Key]
		public int Id { get; set; }
        public int idApplication { get; set; }
		public int idQuestion { get; set; }
	}
    public class QuestionAnswerDTO
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
