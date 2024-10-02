using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class UserModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
