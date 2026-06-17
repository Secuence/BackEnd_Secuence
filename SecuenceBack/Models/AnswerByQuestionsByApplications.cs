using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
    public class AnswersByQuestionsByApplications
    {

        [Key]
        [JsonIgnore]
        public int id { get; set; }
        public int idQuestionsByApplication { get; set; }
        public int idCandidate { get; set; }
        public string answer { get; set; }
    }
}
