using AutoMapper;
using BookExchange.Application.DTOs.Books;
using BookExchange.Application.Exceptions; // Para usar ApplicationException
using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using BookExchange.Application.Contracts;

namespace BookExchange.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ISubjectRepository _subjectRepository; // Necesario para validar SubjectId
        private readonly IStudentRepository _studentRepository; // Necesario para validar OwnerId
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, ISubjectRepository subjectRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _subjectRepository = subjectRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateBookAsync(BookCreateDto createDto)
        {
            // Validar si la materia existe
            var subjectExists = await _subjectRepository.GetByIdAsync(createDto.SubjectId);
            if (subjectExists == null)
            {
                throw new Exceptions.ValidationException($"La materia con ID {createDto.SubjectId} no existe.");
            }

            // Validar si el propietario (ownerId) existe
            var ownerExists = await _studentRepository.GetByIdAsync(createDto.OwnerId);
            if (ownerExists == null)
            {
                throw new Exceptions.ApplicationException($"El propietario (StudentId) con ID {createDto.OwnerId} no existe.");
            }

            // Verificar si el ISBN ya existe para evitar duplicados
            var existingBooksWithIsbn = await _bookRepository.FindAsync(b => b.ISBN == createDto.ISBN);
            if (existingBooksWithIsbn != null && existingBooksWithIsbn.Any())
            {
                throw new Exceptions.ApplicationException($"Ya existe un libro con el ISBN '{createDto.ISBN}'.");
            }


            var book = _mapper.Map<Book>(createDto);

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            // Recargar para incluir las propiedades de navegación para el DTO
            var createdBook = await _bookRepository.GetBookWithDetailsAsync(book.Id);
            return _mapper.Map<BookDto>(createdBook);
        }

        public async Task<BookDto> GetBookByIdAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task UpdateBookAsync(BookUpdateDto updateDto)
        {
            var bookToUpdate = await _bookRepository.GetByIdAsync(updateDto.Id);
            if (bookToUpdate == null)
            {
                throw new KeyNotFoundException($"Libro con ID {updateDto.Id} no encontrado.");
            }

            // Aquí podrías añadir lógica de autorización: solo el propietario puede actualizar su libro
            // if (bookToUpdate.OwnerId != currentUserId) { throw new UnauthorizedAccessException(); }

            // Opcional: Validar si el SubjectId ha cambiado y si existe la nueva materia
            if (updateDto.SubjectId != bookToUpdate.SubjectId)
            {
                var newSubjectExists = await _subjectRepository.GetByIdAsync(updateDto.SubjectId);
                if (newSubjectExists == null)
                {
                    throw new Exceptions.ValidationException($"La nueva materia con ID {updateDto.SubjectId} no existe.");
                }
            }

            // Si el ISBN cambia, verifica que el nuevo ISBN no exista ya
            if (updateDto.ISBN != bookToUpdate.ISBN)
            {
                var existingBookWithNewIsbn = await _bookRepository.FindAsync(b => b.ISBN == updateDto.ISBN);
                if (existingBookWithNewIsbn != null && existingBookWithNewIsbn.Any(b => b.Id != updateDto.Id))
                {
                    throw new Exceptions.ApplicationException($"Ya existe otro libro con el ISBN '{updateDto.ISBN}'.");
                }
            }


            _mapper.Map(updateDto, bookToUpdate); // Mapea los cambios del DTO a la entidad

            _bookRepository.Update(bookToUpdate);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var bookToDelete = await _bookRepository.GetByIdAsync(bookId);
            if (bookToDelete == null)
            {
                throw new KeyNotFoundException($"Libro con ID {bookId} no encontrado.");
            }
          
            _bookRepository.Remove(bookToDelete);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookDto>> GetBooksBySubjectAsync(int subjectId)
        {
            var books = await _bookRepository.GetBooksBySubjectAsync(subjectId);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<BookDto>> GetBooksByOwnerAsync(int ownerId)
        {
            var books = await _bookRepository.GetBooksByOwnerAsync(ownerId);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookWithDetailsAsync(int bookId)
        {
            var book = await _bookRepository.GetBookWithDetailsAsync(bookId);
            return _mapper.Map<BookDto>(book);
        }
    }
}
