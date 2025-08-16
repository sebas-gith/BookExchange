using System.ComponentModel.DataAnnotations;

namespace BookExchange.Application.DTOs.Messages
{
    public class MessageCreateDto
    {
        [Required(ErrorMessage = "El ID del remitente es obligatorio.")]
        public int SenderId { get; set; } 
        [Required(ErrorMessage = "El ID del receptor es obligatorio.")]
        public int ReceiverId { get; set; }

        [Required(ErrorMessage = "El contenido del mensaje es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El mensaje no puede exceder los 1000 caracteres.")]
        public string Content { get; set; }

        public int? ExchangeOfferId { get; set; } 
    }
}
