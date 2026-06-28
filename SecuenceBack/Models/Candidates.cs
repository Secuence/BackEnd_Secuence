using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace SecuenceBack.Models
{
    public class Candidates
    {
        [Key]
        [JsonIgnore]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        [JsonIgnore]
        public int idApplication { get; set; }
        [JsonIgnore]
        public int status { get; set; }
        [JsonIgnore]
        public string? cv { get; set; }
        [JsonIgnore]
        public DateTime createdAt { get; set; }

        public string? comment { get; set; }
        [JsonIgnore]
        public bool? hold { get; set; }
        // campos de metricas //
        //  Fecha de la revision interna //
        [JsonIgnore]
        public DateTime? internalResponseAt { get; set; }
        //  Fecha de la Revision con el cliente //
        [JsonIgnore]
        public DateTime? sendToClientAt { get; set; }
        [JsonIgnore]
        public DateTime? clientResponseAt { get; set; }
        [JsonIgnore]
        public DateTime? clientInterviewAt { get; set; }
        //  Fecha de la  respuesta del ciente //
        [JsonIgnore]
        public DateTime? offerLetterAt { get; set; }
        // Fecha de la Respuesta de la carta de oferta //
        [JsonIgnore]
        public DateTime? candidateResponseAt { get; set; }

        [JsonIgnore]
        public DateTime? deliberyProcessAt { get; set; }
        //  Fecha del Rechazo del candidato//
        [JsonIgnore]
        public DateTime? rejectedAt { get; set; }
        public bool? visible { get; set; }
        public string? activeProcess { get; set; }
        public DateTime? clientaprovAt { get; set; }
        public DateTime? rrhhAT { get; set; }
        public DateTime? internalProcessAt { get; set; }
        public bool? allow { get; set; }
        public DateTime? allowDate { get; set; }
        public bool? interna { get; set; }
    }

    public class Addcoment
    {
        int idcandidate { get; set; }
        string? comment { get; set; }
    }
    public class Answer
    {
        public int idQuestionsByApplication { get; set; }
        public string? answer { get; set; }

    }

    public class CandidateCreate
    {

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public int idApplication { get; set; }
        public string? cv { get; set; }
        public bool? allow { get; set; }
        public DateTime? allowDate { get; set; }
        public bool? interna { get; set; }
    }
    public class CandidateCreateDTO
    {
        public CandidateCreate candidate { get; set; }
        public List<Answer> answer { get; set; }

    }

    public class CandidateDTO
    {

        public int idCandidate { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string? cv { get; set; }
        public int status { get; set; }
        public bool? hold { get; set; }
        public string? activeProcess { get; set; }
        public string? daysInStatus { get; set; }


    }
    public class CandidateGetByIdDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string? cv { get; set; }
        public string? comment { get; set; }
        public int status { get; set; }
        public bool? hold { get; set; }
        public DateTime date { get; set; }
        public bool? hasDelibery { get; set; }
        public string? idDelivery { get; set; }
        public string?  position { get; set; }
        public int? positionid { get; set; } 
        public string? client { get; set; }


        public List<CandidateOtherApplications>? applications { get; set; }

        public List<CandidateQuesition>? questions { get; set; }

        public DateTime createdAt { get; set; }
        
        // timeline
        public List<TimeLineDTO> timeLine { get; set; }
    }

    public class TalentPollDTO
    {
        public int idCandidate { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int status { get; set; }
        public DateTime? created { get; set; }
        [JsonIgnore]
        public int totalRec { get; set; }
        public string? client { get; set; }
        public string? position { get; set; }
        public int positionId { get; set; }
        public int applicationStatus { get; set; }
    }

    public class CandidateByIdDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CV { get; set; }
        public int Status { get; set; }
        public List<QuestionAnswerDTO> QuestionsAndAnswers { get; set; }
    }
    public class TimeLineDTO
    {
        public string name { get; set; }
        public DateTime? time { get; set; }
    }

    public class CandidateQuesition
    {
        public string question { get; set; }
        public string answer { get; set; }
    }
    public class CandidateOtherApplications
    {
        public int IdCandidate { get; set; }
        public int IdApplication { get; set; }
        public string? Position { get; set; }
        public string? Client { get; set; }
        public string? activeProcess { get; set; }
        public int statusCandidate  { get; set; }
        public int? statusApp { get; set; }
        public bool? appHold { get; set; }
        public bool? appHasDelibery { get; set; }
        public string ? appDelibery { get; set; }
    }

}
