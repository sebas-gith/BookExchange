using BookExchange.Application.DTOs.Messages;
using BookExchange.Application.Services;
using BookExchange.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net; // Para HttpStatusCodea
using BookExchange.Application.Contracts;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // POST: api/Messages
        [HttpPost]
        [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] MessageCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // El SenderId viene del DTO
                var message = await _messageService.SendMessageAsync(createDto);
                return CreatedAtAction(nameof(SendMessage), message); // O un endpoint de obtención de mensajes
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Messages/between-users?user1Id=1&user2Id=2
        [HttpGet("between-users")]
        [ProducesResponseType(typeof(IEnumerable<MessageDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesBetweenUsers([FromQuery] int user1Id, [FromQuery] int user2Id)
        {
            if (user1Id <= 0 || user2Id <= 0)
            {
                return BadRequest("Los IDs de usuario son inválidos.");
            }
            try
            {
                var messages = await _messageService.GetMessagesBetweenUsersAsync(user1Id, user2Id);
                return Ok(messages);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("Uno o ambos usuarios no existen.");
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Messages/for-offer/{offerId}
        [HttpGet("for-offer/{offerId}")]
        [ProducesResponseType(typeof(IEnumerable<MessageDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForOffer(int offerId)
        {
            try
            {
                var messages = await _messageService.GetMessagesForExchangeOfferAsync(offerId);
                return Ok(messages);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Oferta de intercambio con ID {offerId} no encontrada.");
            }
        }

        // PATCH: api/Messages/5/read
        [HttpPatch("{id}/read")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            try
            {
                await _messageService.MarkMessageAsReadAsync(id);
                return Ok(new { message = "Mensaje marcado como leído." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        // Si necesitas autorización, podrías pasar el user ID en el query/header para que el servicio lo valide
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _messageService.DeleteMessageAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}