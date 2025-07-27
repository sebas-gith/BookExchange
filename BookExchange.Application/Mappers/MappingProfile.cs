using AutoMapper;
using BookExchange.Domain.Core;
using BookExchange.Domain.Entities;
using BookExchange.Application.DTOs.Students;
using BookExchange.Application.DTOs.Books;
using BookExchange.Application.DTOs.ExchangeOffers;
using BookExchange.Application.DTOs.Messages;
using BookExchange.Application.DTOs.Subjects;

namespace BookExchange.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Student Mappings
            CreateMap<StudentRegisterDto, Student>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // La contraseña se hashea en el servicio

            CreateMap<StudentUpdateDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id se usa para buscar, no para mapear a la entidad directamente

            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate.ToLocalTime())); // Opcional: ajustar a la hora local

            // Book Mappings
            CreateMap<BookCreateDto, Book>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore()); // El ownerId se asigna en el servicio

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.OwnerFirstName, opt => opt.MapFrom(src => src.Owner.FirstName))
                .ForMember(dest => dest.OwnerLastName, opt => opt.MapFrom(src => src.Owner.LastName));

            CreateMap<Subject, SubjectDto>().ReverseMap();

            CreateMap<ExchangeOfferCreateDto, ExchangeOffer>(); 
            CreateMap<ExchangeOffer, ExchangeOfferDto>();

            
            CreateMap<MessageCreateDto, Message>();
            CreateMap<Message, MessageDto>();

            //CreateMap<ReviewCreateDto, Review>();
            
        }
    }
}
