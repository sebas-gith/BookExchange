using System.ComponentModel.DataAnnotations;

namespace BookExchange.Application.DTOs.Reviews
{
    public class ReviewUpdateDto
    {
        [Required(ErrorMessage = "El ID de la reseña es obligatorio para la actualización.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La calificación es obligatoria.")]
        [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5 estrellas.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres.")]
        public string Comment { get; set; }
    }
}