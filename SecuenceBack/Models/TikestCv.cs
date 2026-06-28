namespace SecuenceBack.Models
{
    public class TikestCv
    {
        public int id {  get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? cvExt { get; set; }
        public string? cvInt { get; set; }
        public bool? isTicket { get; set; }
        public string? position { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int status { get; set; }
        public int? idCandiate { get; set; }
        public string? idUser { get; set; }
        public int? language { get; set; }
    }
    public class TikectCvCreateDto
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? cvExt { get; set; }
        public bool? isTicket { get; set; }
        public string? position { get; set; }
        public string? idUser { get; set; }
        public int? language { get; set; }
    }
    public class TikectCvAllDto
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int status { get; set; }
        public string? requesterUser { get; set; }
        public string? cvExt { get; set; }
        public string? lastCV { get;set; }
        public string? language { get; set; }
    }
    public class TiecktCvOneDto
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string cvExt { get; set; }
        public string? cvInt { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int status { get; set; }
        public int? idCandiate { get; set; }
        public string? requesterUser { get; set; }
    }
}

