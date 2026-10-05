using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Persistence.DbModels
{
    [Table("UserRoles", Schema = "Identity")]
    internal class AuthRole : IdentityRole<int> { }
}