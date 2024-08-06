using System.ComponentModel.DataAnnotations;

namespace GymManagment.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime ExpireAt { get; set; }

    }
}
