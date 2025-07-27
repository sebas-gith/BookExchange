using System.ComponentModel.DataAnnotations;

namespace BookExchange.Application.DTOs.Students
{
    public class StudentUpdateDto
    {
        public int Id { get; set; } // Necesario para identificar qué estudiante actualizar

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } // El email podría no ser editable o requerir verificación

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Campus { get; set; }
    }
}