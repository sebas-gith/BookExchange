using BookExchange.Domain.Core;

namespace BookExchange.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int ReviewerId { get; set; } // El estudiante que da la reseña
        public Student Reviewer { get; set; }

        public int ReviewedUserId { get; set; }
        public Student ReviewedUser { get; set; }

        public int Rating { get; set; } // Calificación (ej. 1 a 5 estrellas)
        public string Comment { get; set; } // Comentario de la reseña

        public DateTime ReviewDate { get; set; }

        // Opcional: Relacionar la reseña con una oferta o libro específico
        public int? ExchangeOfferId { get; set; }
        public ExchangeOffer ExchangeOffer { get; set; }

    }
}
