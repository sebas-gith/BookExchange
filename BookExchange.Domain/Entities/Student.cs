using BookExchange.Domain.Core;

namespace BookExchange.Domain.Entities// O BookExchange.Domain.Entities si lo mueves
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Para almacenar el hash de la contraseña
        public string PhoneNumber { get; set; } // Opcional
        public string Campus { get; set; } // Nombre del campus o universidad, si aplica
        public DateTime RegistrationDate { get; set; }

        // Propiedades de navegación (relaciones con otras entidades)
        // Un estudiante puede tener múltiples libros publicados
        public ICollection<Book> BooksPublished { get; set; }

        // Un estudiante puede haber creado múltiples ofertas de intercambio/venta
        public ICollection<ExchangeOffer> ExchangeOffers { get; set; }

        // Un estudiante puede ser el emisor o receptor de múltiples mensajes
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }

        // Un estudiante puede haber recibido múltiples reviews
        public ICollection<Review> ReceivedReviews { get; set; }

        // Un estudiante puede haber dado múltiples reviews
        public ICollection<Review> GivenReviews { get; set; }


        public Student()
        {
            BooksPublished = new HashSet<Book>();
            ExchangeOffers = new HashSet<ExchangeOffer>();
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<Message>();
            ReceivedReviews = new HashSet<Review>();
            GivenReviews = new HashSet<Review>();
        }
    }
}
