using BookExchange.Application.DTOs.ExchangeOffers; // Asegúrate de crear estos DTOs
using BookExchange.Domain.Entities; // Para OfferStatus en el filtro
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Application.Services
{
    public interface IExchangeOfferService
    {
        Task<ExchangeOfferDto> CreateOfferAsync(ExchangeOfferCreateDto createDto, int sellerId);
        Task<ExchangeOfferDto> GetOfferByIdAsync(int offerId);
        Task<IEnumerable<ExchangeOfferDto>> GetAllOffersAsync();
        Task UpdateOfferAsync(ExchangeOfferUpdateDto updateDto);
        Task DeleteOfferAsync(int offerId);
        Task<IEnumerable<ExchangeOfferDto>> GetActiveOffersForBookAsync(int bookId);
        Task<IEnumerable<ExchangeOfferDto>> GetOffersBySellerIdAsync(int sellerId);
        Task<IEnumerable<ExchangeOfferDto>> SearchOffersAsync(OfferSearchDto searchDto); // DTO para filtros de búsqueda
        Task UpdateOfferStatusAsync(int offerId, OfferStatus newStatus);
    }
}
