using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SecuenceBack.Utils
{

    public class Logs
    {
        [Key]
        [JsonIgnore]
        public long id { get; set; }
        public string userName { get; set; }
        public long idModule { get; set; }
        public string moduleName { get; set; }
        public DateTime actionDate { get; set; }
        public string action { get; set; }
        public string message { get; set; }

    }
    public class LogsDTo
    {
        public string? userName { get; set; }
        public string? moduleName { get; set; }
        public string? action { get; set; }
        public string? message { get; set; }
        public DateTime? actionDate { get; set; }
    }
}
