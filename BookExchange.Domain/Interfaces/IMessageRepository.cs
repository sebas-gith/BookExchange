﻿using BookExchange.Domain.Entities; // Necesario para Message

namespace BookExchange.Domain.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        // Métodos específicos para Message
        Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int userId1, int userId2);
        Task<IEnumerable<Message>> GetMessagesForExchangeOfferAsync(int exchangeOfferId);
        Task MarkMessageAsReadAsync(int messageId);
    }
}