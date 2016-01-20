using System.ComponentModel.DataAnnotations;

namespace ML.Web.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string RedirectUrl { get; set; }
    }
}