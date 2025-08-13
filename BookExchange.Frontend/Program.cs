// En BookExchange.Frontend/Program.cs
using BookExchange.Frontend;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BookExchange.Frontend.Services; // �A�adiremos esto pronto!

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://localhost:7165") 
});

// Registrar los servicios que crearemos m�s adelante
builder.Services.AddScoped<BookService>();

await builder.Build().RunAsync();
