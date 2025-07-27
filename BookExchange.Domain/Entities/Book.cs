using BookExchange.Domain.Core;

namespace BookExchange.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; } // Identificador único de libro
        public int PublicationYear { get; set; } // Año de publicación
        public string Edition { get; set; } // Edición del libro (ej. 3ra Edición)
        public string Description { get; set; } // Descripción del estado y detalles del libro

        // Enum para el estado físico del libro
        public BookCondition Condition { get; set; }

        // Clave foránea para la Materia/Categoría
        public int SubjectId { get; set; }
        // Propiedad de navegación a la Materia/Categoría
        public Subject Subject { get; set; }

        // Clave foránea al Student que lo posee/publica
        public int OwnerId { get; set; } // Renombrado de UserId a OwnerId para mayor claridad
        // Propiedad de navegación al Student propietario
        public Student Owner { get; set; }

        // Propiedades de navegación (un libro puede tener múltiples ofertas)
        public ICollection<ExchangeOffer> ExchangeOffers { get; set; }

        public Book()
        {
            ExchangeOffers = new HashSet<ExchangeOffer>();
        }
    }

    // Enum para el estado físico del libro
    public enum BookCondition
    {
        New,        // Nuevo
        LikeNew,    // Como nuevo
        UsedGood,   // Usado (Buen estado)
        UsedFair,   // Usado (Estado regular)
        Worn        // Muy usado
    }
}
