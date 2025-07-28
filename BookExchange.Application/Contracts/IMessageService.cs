using BookExchange.Application.DTOs.Messages; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Application.Contracts
{
    public interface IMessageService
    {
        Task<MessageDto> SendMessageAsync(MessageCreateDto createDto);
        Task<IEnumerable<MessageDto>> GetMessagesBetweenUsersAsync(int userId1, int userId2);
        Task<IEnumerable<MessageDto>> GetMessagesForExchangeOfferAsync(int exchangeOfferId);
        Task MarkMessageAsReadAsync(int messageId);
        Task DeleteMessageAsync(int messageId); 
    }
}
