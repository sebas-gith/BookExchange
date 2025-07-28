using System.ComponentModel.DataAnnotations;

namespace BookExchange.Application.DTOs.Subjects
{
    public class SubjectUpdateDto
    {
        [Required(ErrorMessage = "El ID de la materia es obligatorio para la actualización.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre de la materia no puede exceder los 100 caracteres.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Description { get; set; }
    }
}