using AutoMapper;
using BookExchange.Application.DTOs.ExchangeOffers;
using BookExchange.Application.Exceptions;
using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookExchange.Application.Contracts;

namespace BookExchange.Application.Services
{
    public class ExchangeOfferService : IExchangeOfferService
    {
        private readonly IExchangeOfferRepository _offerRepository;
        private readonly IBookRepository _bookRepository; // Necesario para validar BookId
        private readonly IStudentRepository _studentRepository; // Necesario para validar SellerId
        private readonly IMapper _mapper;

        public ExchangeOfferService(IExchangeOfferRepository offerRepository, IBookRepository bookRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<ExchangeOfferDto> CreateOfferAsync(ExchangeOfferCreateDto createDto)
        {
            // Validar si el libro existe
            var bookExists = await _bookRepository.GetByIdAsync(createDto.BookId);
            if (bookExists == null)
            {
                throw new ValidationException($"El libro con ID {createDto.BookId} no existe.");
            }

            // Validar si el vendedor existe
            var sellerExists = await _studentRepository.GetByIdAsync(createDto.SellerId);
            if (sellerExists == null)
            {
                throw new Exceptions.ApplicationException($"El vendedor (StudentId) con ID {createDto.SellerId} no existe.");
            }

            // Asegurarse de que el libro que se ofrece realmente pertenece al vendedor
            if (bookExists.OwnerId != createDto.SellerId)
            {
                throw new Exceptions.ApplicationException($"El libro con ID {createDto.BookId} no pertenece al vendedor con ID {createDto.SellerId}.");
            }

            var offer = _mapper.Map<ExchangeOffer>(createDto);
            offer.SellerId = createDto.SellerId;
            offer.PostedDate = DateTime.UtcNow;
            offer.Status = OfferStatus.Active; // Por defecto, una nueva oferta está activa

            await _offerRepository.AddAsync(offer);
            await _offerRepository.SaveChangesAsync();

            // Recargar para incluir las propiedades de navegación para el DTO
            var createdOffer = await _offerRepository.GetOfferWithBookAndSellerDetailsAsync(offer.Id);
            return _mapper.Map<ExchangeOfferDto>(createdOffer);
        }

        public async Task<ExchangeOfferDto> GetOfferByIdAsync(int offerId)
        {
            var offer = await _offerRepository.GetOfferWithBookAndSellerDetailsAsync(offerId); // Usar el método con detalles
            return _mapper.Map<ExchangeOfferDto>(offer);
        }

        public async Task<IEnumerable<ExchangeOfferDto>> GetAllOffersAsync()
        {
            // Llama al nuevo método que incluye los detalles
            var offers = await _offerRepository.GetAllOffersWithDetailsAsync();

            // El mapeo ahora funcionará correctamente porque las propiedades de navegación están cargadas
            return _mapper.Map<IEnumerable<ExchangeOfferDto>>(offers);
        }

        public async Task UpdateOfferAsync(ExchangeOfferUpdateDto updateDto)
        {
            var offerToUpdate = await _offerRepository.GetByIdAsync(updateDto.Id);
            if (offerToUpdate == null)
            {
                throw new KeyNotFoundException($"Oferta de intercambio con ID {updateDto.Id} no encontrada.");
            }

            // Aquí podrías añadir lógica de autorización (solo el vendedor puede actualizar su oferta)
            // if (offerToUpdate.SellerId != currentUserId) { throw new UnauthorizedAccessException(); }

            // No permitir cambios si la oferta ya no está activa
            if (offerToUpdate.Status != OfferStatus.Active)
            {
                throw new Exceptions.ApplicationException("No se puede actualizar una oferta que no está activa.");
            }

            // Si el BookId cambia, valida el nuevo libro
            if (updateDto.BookId != offerToUpdate.BookId)
            {
                var newBookExists = await _bookRepository.GetByIdAsync(updateDto.BookId);
                if (newBookExists == null)
                {
                    throw new ValidationException($"El nuevo libro con ID {updateDto.BookId} no existe.");
                }
                if (newBookExists.OwnerId != offerToUpdate.SellerId) // El nuevo libro debe pertenecer al mismo vendedor
                {
                    throw new Exceptions.ApplicationException($"El nuevo libro con ID {updateDto.BookId} no pertenece al vendedor de la oferta.");
                }
            }


            _mapper.Map(updateDto, offerToUpdate);
            _offerRepository.Update(offerToUpdate);
            await _offerRepository.SaveChangesAsync();
        }

        public async Task DeleteOfferAsync(int offerId)
        {
            var offerToDelete = await _offerRepository.GetByIdAsync(offerId);
            if (offerToDelete == null)
            {
                throw new KeyNotFoundException($"Oferta de intercambio con ID {offerId} no encontrada.");
            }
            
            _offerRepository.Remove(offerToDelete);
            await _offerRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExchangeOfferDto>> GetActiveOffersForBookAsync(int bookId)
        {
            var offers = await _offerRepository.GetActiveOffersForBookAsync(bookId);
            return _mapper.Map<IEnumerable<ExchangeOfferDto>>(offers);
        }

        public async Task<IEnumerable<ExchangeOfferDto>> GetOffersBySellerIdAsync(int sellerId)
        {
            var offers = await _offerRepository.GetOffersBySellerIdAsync(sellerId);
            return _mapper.Map<IEnumerable<ExchangeOfferDto>>(offers);
        }

        public async Task<IEnumerable<ExchangeOfferDto>> SearchOffersAsync(OfferSearchDto searchDto)
{
    var offers = await _offerRepository.GetOffersFilteredAsync(
        searchDto.MinPrice,
        searchDto.MaxPrice,
        searchDto.SubjectId,
        searchDto.Condition,
        searchDto.Type,
        searchDto.Status,
        searchDto.Keywords);
    
    return _mapper.Map<IEnumerable<ExchangeOfferDto>>(offers);
}

        public async Task UpdateOfferStatusAsync(int offerId, OfferStatus newStatus)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                throw new KeyNotFoundException($"Oferta de intercambio con ID {offerId} no encontrada.");
            }

            // Validar transiciones de estado (ej. no de Sold a Active)
            if (offer.Status == OfferStatus.Sold || offer.Status == OfferStatus.Exchanged)
            {
                if (newStatus == OfferStatus.Active)
                {
                    throw new Exceptions.ApplicationException("No se puede reactivar una oferta que ya ha sido completada.");
                }
            }

                 offer.Status = newStatus;
            _offerRepository.Update(offer);
            await _offerRepository.SaveChangesAsync();
        }
    }

}