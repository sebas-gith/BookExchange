﻿using System.ComponentModel.DataAnnotations;
using BookExchange.Domain.Entities; // Para OfferType

namespace BookExchange.Application.DTOs.ExchangeOffers
{
    public class ExchangeOfferCreateDto
    {
        [Required(ErrorMessage = "El ID del vendedor es obligatorio.")]
        public int SellerId { get; set; } 
        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "El tipo de oferta es obligatorio.")]
        public OfferType Type { get; set; }

        [Range(0.00, 99999.99, ErrorMessage = "El precio debe estar entre 0 y 99999.99.")]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción de libros deseados no puede exceder los 1000 caracteres.")]
        public string DesiredBooksForExchange { get; set; }

        [StringLength(250, ErrorMessage = "La ubicación no puede exceder los 250 caracteres.")]
        public string Location { get; set; }

        public DateTime? ExpirationDate { get; set; } // Opcional
    }
}