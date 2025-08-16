using BookExchange.Application.DTOs.ExchangeOffers;
using BookExchange.Application.Services;
using BookExchange.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net; // Para HttpStatusCode
using BookExchange.Domain.Entities; // Para OfferStatus en UpdateStatus
using BookExchange.Application.Contracts;

namespace BookExchange.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeOffersController : ControllerBase
    {
        private readonly IExchangeOfferService _offerService;

        public ExchangeOffersController(IExchangeOfferService offerService)
        {
            _offerService = offerService;
        }

        // GET: api/ExchangeOffers
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExchangeOfferDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ExchangeOfferDto>>> GetExchangeOffers()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return Ok(offers);
        }

        // GET: api/ExchangeOffers/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExchangeOfferDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ExchangeOfferDto>> GetExchangeOffer(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return Ok(offer);
        }

        // GET: api/ExchangeOffers/by-book/{bookId}
        [HttpGet("by-book/{bookId}")]
        [ProducesResponseType(typeof(IEnumerable<ExchangeOfferDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ExchangeOfferDto>>> GetOffersByBook(int bookId)
        {
            var offers = await _offerService.GetActiveOffersForBookAsync(bookId);
            return Ok(offers);
        }

        // GET: api/ExchangeOffers/by-seller/{sellerId}
        [HttpGet("by-seller/{sellerId}")]
        [ProducesResponseType(typeof(IEnumerable<ExchangeOfferDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ExchangeOfferDto>>> GetOffersBySeller(int sellerId)
        {
            var offers = await _offerService.GetOffersBySellerIdAsync(sellerId);
            return Ok(offers);
        }

        // GET: api/ExchangeOffers/search
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ExchangeOfferDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<ExchangeOfferDto>>> SearchOffers([FromQuery] OfferSearchDto searchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var offers = await _offerService.SearchOffersAsync(searchDto);
            return Ok(offers);
        }

        // POST: api/ExchangeOffers
        [HttpPost]
        [ProducesResponseType(typeof(ExchangeOfferDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateExchangeOffer([FromBody] ExchangeOfferCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // El SellerId viene en el DTO en este escenario sin JWT
                var offer = await _offerService.CreateOfferAsync(createDto);
                return CreatedAtAction(nameof(GetExchangeOffer), new { id = offer.Id }, offer);
            }
            catch (Application.Exceptions.ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/ExchangeOffers/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)] 
        public async Task<IActionResult> UpdateExchangeOffer(int id, [FromBody] ExchangeOfferUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la oferta en el cuerpo.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _offerService.UpdateOfferAsync(updateDto);
                return Ok(new { message = "Oferta actualizada exitosamente." });
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

         [HttpPatch("{id}/status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateOfferStatus(int id, [FromQuery] OfferStatus newStatus, [FromQuery] int sellerId)
        {
          
            try
            {
                await _offerService.UpdateOfferStatusAsync(id, newStatus);
                return Ok(new { message = $"Estado de la oferta {id} actualizado a {newStatus}." });
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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)] 
        public async Task<IActionResult> DeleteExchangeOffer(int id)
        {
            try
            {
                await _offerService.DeleteOfferAsync(id);
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