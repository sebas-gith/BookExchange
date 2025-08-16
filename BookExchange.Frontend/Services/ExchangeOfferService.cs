// BookExchange.Client.Services/ExchangeOfferService.cs

using BookExchange.Application.DTOs.ExchangeOffers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities; // Necesitas agregar esta referencia

namespace BookExchange.Frontend.Services
{
    public class ExchangeOfferService
    {
        private readonly HttpClient _httpClient;

        public ExchangeOfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExchangeOfferDto>> SearchOffersAsync(OfferSearchDto searchDto)
        {
            var queryParams = new Dictionary<string, string>();
            var offers = new List<ExchangeOfferDto>();

            if (searchDto.MinPrice.HasValue)
                queryParams["minPrice"] = searchDto.MinPrice.ToString();

            if (searchDto.MaxPrice.HasValue)
                queryParams["maxPrice"] = searchDto.MaxPrice.ToString();

            if (searchDto.SubjectId.HasValue)
                queryParams["subjectId"] = searchDto.SubjectId.ToString();

            if (searchDto.Condition.HasValue)
                queryParams["condition"] = searchDto.Condition.ToString();

            if (searchDto.Type.HasValue)
                queryParams["type"] = searchDto.Type.ToString();

            if (!string.IsNullOrEmpty(searchDto.Keywords))
                queryParams["keywords"] = searchDto.Keywords;

            // Si no hay ningún filtro, obtenemos todas las ofertas directamente para evitar la llamada de búsqueda
            if (queryParams.Count == 0)
            {
                offers = await GetAllOffersAsync();
            }
            else
            {
                var uri = QueryHelpers.AddQueryString("api/exchangeoffers/search", queryParams);
                offers = await _httpClient.GetFromJsonAsync<List<ExchangeOfferDto>>(uri);
            }

            // Si la búsqueda no encontró coincidencias, devuelve todas las ofertas
            if (offers == null || offers.Count == 0)
            {
                return await GetAllOffersAsync();
            }

            return offers;
        }
        public async Task CreateOfferAsync(ExchangeOfferCreateDto createDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/exchangeoffers", createDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ExchangeOfferDto>> GetAllOffersAsync()
        {
            // Asume que tienes un endpoint en tu API para obtener todas las ofertas.
            return await _httpClient.GetFromJsonAsync<List<ExchangeOfferDto>>("api/ExchangeOffers");
        }
    }
}