using System.ComponentModel.DataAnnotations;

namespace KPImanDental.Dto
{
    public class UserTokenDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserToken { get; set; }
    }
}
