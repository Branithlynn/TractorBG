using System.ComponentModel.DataAnnotations;

namespace TractorBG.Model
{
    public class loginRegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}
