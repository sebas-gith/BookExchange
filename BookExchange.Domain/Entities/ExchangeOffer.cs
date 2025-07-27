using BookExchange.Domain.Core;

namespace BookExchange.Domain.Entities
{
    public class ExchangeOffer : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; } // Propiedad de navegación al libro que se ofrece

        public int SellerId { get; set; } // El estudiante que hace la oferta
        public Student Seller { get; set; } // Propiedad de navegación al estudiante

        public OfferType Type { get; set; } // Venta, Intercambio, Ambos

        public decimal Price { get; set; } // Precio si es de venta (puede ser 0 para intercambio)
        public string DesiredBooksForExchange { get; set; } // Si es intercambio, qué libros le interesan
        public string Location { get; set; } // Ubicación de preferencia para el intercambio/venta

        public DateTime PostedDate { get; set; }
        public DateTime? ExpirationDate { get; set; } // Fecha opcional de expiración de la oferta

        public OfferStatus Status { get; set; } // Estado de la oferta (Activa, Vendida, Intercambiada, Cancelada)

        // Propiedad de navegación para mensajes relacionados con esta oferta
        public ICollection<Message> RelatedMessages { get; set; }

        public ExchangeOffer()
        {
            RelatedMessages = new HashSet<Message>();
        }
    }

    // Enum para el tipo de oferta
    public enum OfferType
    {
        Sale,       // Solo venta
        Exchange,   // Solo intercambio
        Both        // Venta o intercambio
    }

    // Enum para el estado de la oferta
    public enum OfferStatus
    {
        Active,         // Oferta activa y disponible
        Sold,           // Libro vendido
        Exchanged,      // Libro intercambiado
        Pending,        // Alguna transacción en curso (opcional)
        Cancelled,      // Oferta cancelada por el vendedor
        Expired         // Oferta caducada
    }
}
