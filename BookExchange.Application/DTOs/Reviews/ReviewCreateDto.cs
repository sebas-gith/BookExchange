using System.ComponentModel.DataAnnotations;

namespace BookExchange.Application.DTOs.Reviews
{
    public class ReviewCreateDto
    {
        [Required(ErrorMessage = "El ID del revisor es obligatorio.")]
        public int ReviewerId { get; set; } // Añadimos esto
        [Required(ErrorMessage = "El ID del usuario a reseñar es obligatorio.")]
        public int ReviewedUserId { get; set; }

        [Required(ErrorMessage = "La calificación es obligatoria.")]
        [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5 estrellas.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres.")]
        public string Comment { get; set; }

        public int? ExchangeOfferId { get; set; } // Opcional: ID de la oferta relacionada
    }
}
