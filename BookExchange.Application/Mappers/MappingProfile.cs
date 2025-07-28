using AutoMapper;
using BookExchange.Domain.Core;
using BookExchange.Domain.Entities;
using BookExchange.Application.DTOs.Students;
using BookExchange.Application.DTOs.Books;
using BookExchange.Application.DTOs.Subjects;
using BookExchange.Application.DTOs.ExchangeOffers; 
using BookExchange.Application.DTOs.Messages;
using BookExchange.Application.DTOs.Reviews; 

namespace BookExchange.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Student Mappings
            CreateMap<StudentRegisterDto, Student>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<StudentUpdateDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Id se usa para buscar, no para mapear a la entidad directamente

            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate.ToLocalTime()));

            // Book Mappings
            CreateMap<BookCreateDto, Book>()
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore());

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.OwnerFirstName, opt => opt.MapFrom(src => src.Owner.FirstName))
                .ForMember(dest => dest.OwnerLastName, opt => opt.MapFrom(src => src.Owner.LastName));

            // BookUpdateDto -> Book
            CreateMap<BookCreateDto, Book>(); // Ya no ignoramos OwnerId
                                              // Asegúrate de que BookUpdateDto también se mapee correctamente
            CreateMap<BookUpdateDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            // Subject Mappings
            CreateMap<SubjectCreateDto, Subject>();
            CreateMap<SubjectUpdateDto, Subject>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Subject, SubjectDto>().ReverseMap(); // Mapeo bidireccional simple para entidades básicas

            // ExchangeOffer Mappings
            CreateMap<ExchangeOfferCreateDto, ExchangeOffer>()
                .ForMember(dest => dest.SellerId, opt => opt.Ignore())
                .ForMember(dest => dest.PostedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OfferStatus.Active)); // Estado inicial

            CreateMap<ExchangeOfferUpdateDto, ExchangeOffer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ExchangeOffer, ExchangeOfferDto>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller));

            // Message Mappings
            CreateMap<MessageCreateDto, Message>()
                .ForMember(dest => dest.SenderId, opt => opt.Ignore())
                .ForMember(dest => dest.SentDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
                .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver))
                .ForMember(dest => dest.ExchangeOffer, opt => opt.MapFrom(src => src.ExchangeOffer));

            // Review Mappings
            CreateMap<ReviewCreateDto, Review>()
                .ForMember(dest => dest.ReviewerId, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<ReviewUpdateDto, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Reviewer, opt => opt.MapFrom(src => src.Reviewer))
                .ForMember(dest => dest.ReviewedUser, opt => opt.MapFrom(src => src.ReviewedUser))
                .ForMember(dest => dest.ExchangeOffer, opt => opt.MapFrom(src => src.ExchangeOffer));
        }
    }
}