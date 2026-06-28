namespace SecuenceBack.Models
{
    public class Tags
    {
        public long id { get; set; }
        public int idSolicitud { get; set; }
        public string TagName { get; set; }
    }
    public class tagsCreate
    { 
        public string TagName { get; set; }
    }
}
