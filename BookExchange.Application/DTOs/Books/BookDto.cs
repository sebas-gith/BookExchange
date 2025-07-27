using BookExchange.Domain.Entities; // Para el enum BookCondition

namespace BookExchange.Application.DTOs.Books
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public string Edition { get; set; }
        public string Description { get; set; }
        public BookCondition Condition { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } // Para mostrar el nombre de la materia
        public int OwnerId { get; set; }
        public string OwnerFirstName { get; set; } // Para mostrar el nombre del propietario
        public string OwnerLastName { get; set; }
    }
}