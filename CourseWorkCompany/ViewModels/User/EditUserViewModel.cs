using System.ComponentModel.DataAnnotations;

namespace CourseWorkCompany.ViewModels.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
        [Display(Name = "Пароль")]
        public string NewPassword { get; set; }
    }
}
