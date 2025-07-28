using BookExchange.Application.DTOs.Subjects;
using BookExchange.Application.Services;
using BookExchange.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using BookExchange.Application.Contracts;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/Subjects
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubjectDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            return Ok(subjects);
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SubjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<SubjectDto>> GetSubject(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        // POST: api/Subjects
        // Solo para administradores o un sistema de gestión interno
        [HttpPost]
        [ProducesResponseType(typeof(SubjectDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var subject = await _subjectService.CreateSubjectAsync(createDto);
                return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Subjects/5
        // Solo para administradores o un sistema de gestión interno
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la materia en el cuerpo.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _subjectService.UpdateSubjectAsync(updateDto);
                return Ok(new { message = "Materia actualizada exitosamente." });
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

        // DELETE: api/Subjects/5
        // Solo para administradores o un sistema de gestión interno
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] // Si no se puede eliminar por libros asociados
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await _subjectService.DeleteSubjectAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Application.Exceptions.ApplicationException ex) // Captura si hay libros asociados
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}