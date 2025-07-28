using BookExchange.Application.DTOs.Students;
using BookExchange.Application.Services;
using BookExchange.Application.Exceptions;
using BookExchange.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookExchange.Application.Contracts;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto studentDto)
        {
            if (id != studentDto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _studentService.UpdateStudentAsync(studentDto);
                return Ok(new { message = "Perfil actualizado exitosamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
           
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}