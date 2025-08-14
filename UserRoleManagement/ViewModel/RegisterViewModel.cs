using System.ComponentModel.DataAnnotations;

namespace UserRoleManagement.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Name is required")]
        [Display(Name ="Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password does not match")]
        [Display(Name ="Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
