// En BookExchange.Frontend/Program.cs
using Blazored.LocalStorage;
using BookExchange.Frontend;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BookExchange.Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BookExchange.Application.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7165")
});

// Registra los servicios de autenticación
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthService>(); // Tu servicio de autenticación
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<ExchangeOfferService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<SubjectService>();

await builder.Build().RunAsync();