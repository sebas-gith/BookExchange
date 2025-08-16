using BookExchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookExchange.Application.DTOs.Books
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
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

        // Clave foránea al Student que lo posee/publica
        public int OwnerId { get; set; } // Renombrado de UserId a OwnerId para mayor claridad
        // Propiedad de navegación al Student propietario

    }
}
