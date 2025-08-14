using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UserRoleManagement.Data
{
    public class Users : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
    }
}
