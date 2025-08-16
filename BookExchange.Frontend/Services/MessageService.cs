// BookExchange.Client.Services/MessageService.cs

using BookExchange.Application.DTOs.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web; // Necesario para HttpUtility.ParseQueryString

namespace BookExchange.Frontend.Services
{
    public class MessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendMessageAsync(MessageCreateDto createDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/messages", createDto);
            response.EnsureSuccessStatusCode();
        }

        // Método para obtener mensajes entre dos usuarios
        public async Task<IEnumerable<MessageDto>> GetConversationAsync(int user1Id, int user2Id)
        {
            var uri = $"api/messages/between-users?user1Id={user1Id}&user2Id={user2Id}";
            return await _httpClient.GetFromJsonAsync<IEnumerable<MessageDto>>(uri);
        }

        // Método para obtener la bandeja de entrada de un usuario (mensajes enviados y recibidos)
        public async Task<IEnumerable<MessageDto>> GetMessagesForUserAsync(int userId)
        {
            // Este endpoint no existe en tu controlador, por lo que usaremos una llamada combinada
            // a los otros endpoints para simular la bandeja de entrada
            // *Ojo: Esto es una solución temporal hasta que crees un endpoint dedicado en el backend.*
            var sentMessages = await _httpClient.GetFromJsonAsync<IEnumerable<MessageDto>>($"api/messages/by-sender/{userId}");
            var receivedMessages = await _httpClient.GetFromJsonAsync<IEnumerable<MessageDto>>($"api/messages/by-receiver/{userId}");

            var allMessages = new List<MessageDto>();
            if (sentMessages != null) allMessages.AddRange(sentMessages);
            if (receivedMessages != null) allMessages.AddRange(receivedMessages);

            return allMessages;
        }
    }
}