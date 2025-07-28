using BookExchange.Application.DTOs.Books;
using BookExchange.Application.Services;
using BookExchange.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookExchange.Application.Contracts;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookService.GetBookWithDetailsAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDto createDto)
        {
            // El OwnerId ahora viene del DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var book = await _bookService.CreateBookAsync(createDto); // Sin ownerId separado
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Mantenemos este tipo de respuesta si el servicio lanza UnauthorizedAccessException
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bookService.UpdateBookAsync(updateDto); // El servicio se encargará de la validación del propietario
                return Ok(new { message = "Libro actualizado exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex) // Captura la excepción de autorización del servicio
            {
                return Forbid(ex.Message); // Retorna 403 Forbidden
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteBook(int id)
        {
           
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
