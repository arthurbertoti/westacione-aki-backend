using WEstacionaAPI.DbContexto;

var builder = WebApplication.CreateBuilder(args);

// Adiciona a classe Startup como respons�vel pela configura��o da aplica��o
builder.Services.AddControllers();

// Cria a inst�ncia de Startup e chama os m�todos de configura��o de servi�os
var startup = new WEstacionaAPI.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

builder.Services.AddScoped<Usuario>(); // AddTransient ou AddSingleton podem ser usados conforme a necessidade  
var app = builder.Build();

// Chama o m�todo de configura��o do pipeline de requisi��o HTTP
startup.Configure(app, app.Environment);

app.Run();
