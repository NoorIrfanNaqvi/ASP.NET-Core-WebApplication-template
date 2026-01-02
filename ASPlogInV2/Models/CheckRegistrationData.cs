using System.ComponentModel.DataAnnotations;

namespace ASPlogInV2.Models
{
    public class CheckRegistrationData
    {
        [Key]
        public string? username {  get; set; }
        public string? useremail { get; set; }
        public string? password { get; set; }
        public string? confirmpassword { get; set; }
        public bool agreedToTerms { get; set; }
    }
}
