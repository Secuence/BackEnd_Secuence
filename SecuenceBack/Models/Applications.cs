using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
    public class Applications
    {
        [Key]
        [JsonIgnore]
        public int id { get; set; }
        public int IdManager { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string reporting { get; set; }
        public string description { get; set; }
        public bool newPosition { get; set; }
        public string? whoReplaces { get; set; }
        public string requestingClient { get; set; }
        public string workModality { get; set; }
        public string? hybridModality { get; set; } // pregunta que info va y reglas para este
        public string? typeContract { get; set; }
        public string? salaryRange { get; set; }
        public DateTime? entryDate { get; set; }
        public string assignmentTime { get; set; }  //preguntar
        public string education { get; set; }
        public string yearsExperience { get; set; }
        public string englishLevel { get; set; }
        public string? technicalSkills { get; set; }
        public string? certifications { get; set; }
        public string? additionalRequirement { get; set; }

        /// <summary>
        /// nuevos campos
        /// </summary>
        public int? idRecruiter { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
        public int? limit { get; set; } 
        public int? status { get; set; }
        public string? comment { get; set; }
        public string? titleVacant { get; set; }
        public string? vacant { get; set; }
        // metricas //
        [JsonIgnore]
        public DateTime? approvedAt { get; set; }
        [JsonIgnore]
        public DateTime? refusedAt { get; set; }
        [JsonIgnore]
        public DateTime? publishedAt { get; set; }
        [JsonIgnore]
        public DateTime? closedAt { get; set; }
        
        public bool? isFreezed { get; set; }
        public DateTime? freezeAt { get; set; }

        public string? idDelivery { get; set; }
        public bool? hasDelibery { get; set; }
        public int? idIcon { get; set; }
    }
    public class ApplicationsGetOne
    {
        [Key]
        [JsonIgnore]
        public int id { get; set; }
        public int IdManager { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string reporting { get; set; }
        public string description { get; set; }
        public bool newPosition { get; set; }
        public string? whoReplaces { get; set; }
        public string requestingClient { get; set; }
        public string workModality { get; set; }
        public string? hybridModality { get; set; } // pregunta que info va y reglas para este
        public string? typeContract { get; set; }
        public string? salaryRange { get; set; }
        public DateTime? entryDate { get; set; }
        public string assignmentTime { get; set; }  //preguntar
        public string education { get; set; }
        public string yearsExperience { get; set; }
        public string englishLevel { get; set; }
        public string? technicalSkills { get; set; }
        public string? certifications { get; set; }
        public string? additionalRequirement { get; set; }

        /// <summary>
        /// nuevos campos
        /// </summary>
        public int? idRecruiter { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
        public int? limit { get; set; }
        public int? status { get; set; }
        public string? comment { get; set; }
        public string? titleVacant { get; set; }
        public string? vacant { get; set; }
        // metricas //
        [JsonIgnore]
        public DateTime? approvedAt { get; set; }
        [JsonIgnore]
        public DateTime? refusedAt { get; set; }
        [JsonIgnore]
        public DateTime? publishedAt { get; set; }
        [JsonIgnore]
        public DateTime? closedAt { get; set; }

        public bool? isFreezed { get; set; }
        public DateTime? freezeAt { get; set; }

        public string? idDelivery { get; set; }
        public bool? hasDelibery { get; set; }
        public string? recruiterName { get; set; }
      public string? idIcon { get; set; }
    }

    public class AplicationCreateDTO
    {
        public Applications applications { get; set; }
        public List<string>? tags {get;set;}
    }
    public class AplicationGetAllDTO
    {
        public int id { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string client { get; set; }
        public int  limit { get; set; }
        public int? status { get; set; }
        public List<Tags>? tags { get; set; }
    }

    public class VacantGetAllDTO
    {
        public int id { get; set; }
        public string titleVacant { get; set; }
        public string vacant { get; set; }
        public int? status { get; set; }
        public string client { get; set; }
        public bool? isFreeze { get; set; }
        public string? technicalSkills { get; set; }
        public string? vacantIcon { get; set; }
        public List<Tags>? tags { get; set; }
    }

    public class AplicationDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public int? status { get; set; }
        public DateTime? createdAt { get; set; }
        public List<CandidateDTO> candidates { get; set; }
    }

    public class VacantValidateDTO
    {
        public int id { get; set; }
        public string vacantHtml { get; set; }
        public string title { get; set; }
        public int? idIcon { get; set; }
        public List<Questions> questions { get; set; }
    }

}
