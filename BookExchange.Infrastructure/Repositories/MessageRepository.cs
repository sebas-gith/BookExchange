using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BookExchange.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(BookExchangeContext context) : base(context)
        {
        }

        
        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int userId1, int userId2)
        {
            return await _dbSet
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesForExchangeOfferAsync(int exchangeOfferId)
        {
            return await _dbSet
                .Where(m => m.ExchangeOfferId == exchangeOfferId)
                .OrderBy(m => m.SentDate)
                .ToListAsync();
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _dbSet.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                
            }
        }
    }
}
