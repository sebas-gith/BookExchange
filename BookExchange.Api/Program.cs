using Microsoft.EntityFrameworkCore;
using BookExchange.Infrastructure.Context;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Repositories;
using BookExchange.Application.Services;
using BookExchange.Application.Mappers;
using BookExchange.Application.Contracts;
using BookExchange.Application;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BookExchangeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // Genérico
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IExchangeOfferRepository, ExchangeOfferRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IExchangeOfferService, ExchangeOfferService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (BookExchange.Application.Exceptions.ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { errors = ex.Errors, message = ex.Message });
    }
    catch (KeyNotFoundException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message });
    }
    catch (BookExchange.Application.Exceptions.ApplicationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An unhandled exception has occurred.");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde." });
    }
});


app.MapControllers();

app.Run();