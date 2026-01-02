using System.ComponentModel.DataAnnotations;

namespace ASPlogInV2.Models
{
    public class userAccounts
    {
        public string UserEmail { get; set; }
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
