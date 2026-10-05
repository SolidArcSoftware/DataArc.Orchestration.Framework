using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Persistence.DbModels
{
    [Table("AspNetUsers")]
    public class AuthUser : IdentityUser<int>
    {
        public string? UserTimeZone { get; set; }
    }
}