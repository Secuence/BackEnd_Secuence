using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace SecuenceBack.Models
{
    public class UserTbl
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Key]
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Country { get; set; }
        public string? Photo { get; set; }
        public string? UserType { get; set; }
        public int PolicesAccepted { get; set; }
        public int Status { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? DeletedAt { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? PolicesAcceptedAt { get; set; }

    }
    public class UserCreate
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Country { get; set; }
        public string? Photo { get; set; }
        public string? UserType { get; set; }
        public int PolicesAccepted { get; set; }

    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdate
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Country { get; set; }
        public string? Photo { get; set; }
        public string? UserType { get; set; }
        public int Status { get; set; }
    }

    public class UserDto
    {
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? Country { get; set; }
        public string? UserType { get; set; }
        public string? StatusName { get; set; }

    }
    public class contexttrue
    {
        public string email { get; set; }
        public bool ifuserexist { get; set; }
        //public List<string> permissions { get; set; }
    }
    public class contextfalse
    {
        public string token { get; set; }

        public string firstname { get; set; }
        public string email { get; set; }
        public bool ifuserexist { get; set; }
    }

}
