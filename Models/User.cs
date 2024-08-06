using GymManagment.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagment.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        // [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
