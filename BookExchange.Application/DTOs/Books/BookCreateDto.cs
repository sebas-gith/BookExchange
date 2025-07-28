using BookExchange.Domain.Entities; // Para el enum BookCondition
using System.ComponentModel.DataAnnotations;

namespace BookExchange.Application.DTOs.Books
{
    public class BookCreateDto
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        [StringLength(250)]
        public string Author { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 10)] // ISBN puede ser de 10 o 13 dígitos
        public string ISBN { get; set; }

        [Required]
        [Range(1000, 2025)] // Ajustar el año máximo según el año actual
        public int PublicationYear { get; set; }

        [StringLength(100)]
        public string Edition { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public BookCondition Condition { get; set; }

        [Required]
        public int SubjectId { get; set; }

        // El OwnerId se obtendría del usuario autenticado, no se pasa en el DTO de creación
    }
}