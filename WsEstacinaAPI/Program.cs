using WEstacionaAPI.DbContexto;

var builder = WebApplication.CreateBuilder(args);

// Adiciona a classe Startup como responsável pela configuração da aplicação
builder.Services.AddControllers();

// Cria a instância de Startup e chama os métodos de configuração de serviços
var startup = new WEstacionaAPI.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

builder.Services.AddScoped<Usuario>(); // AddTransient ou AddSingleton podem ser usados conforme a necessidade  
var app = builder.Build();

// Chama o método de configuração do pipeline de requisição HTTP
startup.Configure(app, app.Environment);

app.Run();
