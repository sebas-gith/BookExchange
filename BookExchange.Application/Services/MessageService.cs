using AutoMapper;
using BookExchange.Application.DTOs.Messages;
using BookExchange.Application.Exceptions;
using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookExchange.Application.Contracts;

namespace BookExchange.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IStudentRepository _studentRepository; // Para validar remitente y receptor
        private readonly IExchangeOfferRepository _offerRepository; // Para validar la oferta
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IStudentRepository studentRepository, IExchangeOfferRepository offerRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _studentRepository = studentRepository;
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<MessageDto> SendMessageAsync(MessageCreateDto createDto)
        {
            // Validar remitente y receptor
            var sender = await _studentRepository.GetByIdAsync(createDto.SenderId);
            if (sender == null) throw new Exceptions.ApplicationException($"Remitente (StudentId) con ID {createDto.SenderId} no encontrado.");

            var receiver = await _studentRepository.GetByIdAsync(createDto.ReceiverId);
            if (receiver == null) throw new Exceptions.ApplicationException($"Receptor (StudentId) con ID {createDto.ReceiverId} no encontrado.");

            // Validar si la oferta existe si se proporciona
            if (createDto.ExchangeOfferId.HasValue)
            {
                var offerExists = await _offerRepository.GetByIdAsync(createDto.ExchangeOfferId.Value);
                if (offerExists == null)
                {
                    throw new ValidationException($"La oferta de intercambio con ID {createDto.ExchangeOfferId.Value} no existe.");
                }
            }

            var message = _mapper.Map<Message>(createDto);
            message.SenderId = createDto.SenderId;
            message.SentDate = DateTime.UtcNow;
            message.IsRead = false; // Por defecto no leído

            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
            return _mapper.Map<MessageDto>(message);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesBetweenUsersAsync(int userId1, int userId2)
        {
            // Verificar si los usuarios existen
            var user1 = await _studentRepository.GetByIdAsync(userId1);
            if (user1 == null) throw new KeyNotFoundException($"Usuario con ID {userId1} no encontrado.");
            var user2 = await _studentRepository.GetByIdAsync(userId2);
            if (user2 == null) throw new KeyNotFoundException($"Usuario con ID {userId2} no encontrado.");

            var messages = await _messageRepository.GetMessagesBetweenUsersAsync(userId1, userId2);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForExchangeOfferAsync(int exchangeOfferId)
        {
            // Verificar si la oferta existe
            var offer = await _offerRepository.GetByIdAsync(exchangeOfferId);
            if (offer == null) throw new KeyNotFoundException($"Oferta con ID {exchangeOfferId} no encontrada.");

            var messages = await _messageRepository.GetMessagesForExchangeOfferAsync(exchangeOfferId);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            await _messageRepository.MarkMessageAsReadAsync(messageId);
            await _messageRepository.SaveChangesAsync(); // Guardar el cambio
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var messageToDelete = await _messageRepository.GetByIdAsync(messageId);
            if (messageToDelete == null)
            {
                throw new KeyNotFoundException($"Mensaje con ID {messageId} no encontrado.");
            }
           
            _messageRepository.Remove(messageToDelete);
            await _messageRepository.SaveChangesAsync();
        }
    }
}