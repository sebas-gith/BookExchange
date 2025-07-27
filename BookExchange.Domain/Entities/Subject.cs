using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BookExchange.Domain.Core;
using System.Collections.Generic;

namespace BookExchange.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } // Opcional, para dar más detalles sobre la materia

        // Propiedad de navegación (una materia puede tener muchos libros)
        public ICollection<Book> Books { get; set; }

        public Subject()
        {
            Books = new HashSet<Book>();
        }
    }
}
