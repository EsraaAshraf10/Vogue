using System.ComponentModel.DataAnnotations;

namespace VogueApis.DTOs
{
    public class RegisterDto
    {
        [Required]
      
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
