using PokedexWeb.BusinessWorkflow;
using PokedexWeb.DataAccess;
using PokedexWeb.ServiceGateway;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IPokeApiServicio, PokeApiServicio>(client =>
{
    var baseUrl = builder.Configuration["PokeApi:UrlBase"];
    if (!string.IsNullOrEmpty(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
    }
    else
    {
        throw new InvalidOperationException("La configuración 'PokeApi:UrlBase' no está configurada.");
    }
    
    client.Timeout = TimeSpan.FromSeconds(15);
});

// Inyección de dependencias para el flujo de negocio y el repositorio
builder.Services.AddScoped<IPokemonFlujo, PokemonFlujo>();
builder.Services.AddScoped<IEquipoFlujo, EquipoFlujo>();

// Inyección de dependencias para el repositorio de equipo DA
builder.Services.AddScoped<IEquipoRepositorio, EquipoRepositorio>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pokedex}/{action=Index}/{id?}");

app.Run();
