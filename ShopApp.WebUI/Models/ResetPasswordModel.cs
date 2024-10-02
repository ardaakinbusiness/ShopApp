using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
}
