using GymManagment.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagment.Models.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        public Role Role { get; set; } = 0;
        
        public IFormFile Image { get; set; }

    }
}
