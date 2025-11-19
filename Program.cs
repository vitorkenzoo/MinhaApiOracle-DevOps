using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Data;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ===================================================================================
// CONFIGURAÇÃO DO BANCO DE DADOS (.NET 9 e SQL Server)
// ===================================================================================
var connectionString = builder.Configuration.GetConnectionString("SqlAzureConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    var errorMessage = 
        "Connection string 'SqlAzureConnection' não foi configurada.\n" +
        "Configure a connection string:\n" +
        "1. No Azure Portal: App Service > Configuration > Application settings\n" +
        "   Adicione: ConnectionStrings__SqlAzureConnection\n" +
        "2. Ou via Azure CLI: az webapp config appsettings set\n" +
        "3. Ou via Pipeline: configure a variável SQL_CONNECTION_STRING\n" +
        "Veja INSTRUCOES-CONFIGURACAO.md para mais detalhes.";
    
    throw new InvalidOperationException(errorMessage);
}

builder.Services.AddDbContext<AppDb>(options =>
    options.UseSqlServer(connectionString));

// --- Configuração de Serviços ---

// ===================================================================================
// CORREÇÃO (Erro 500 no GET):
// Adiciona o AddJsonOptions para ignorar "loops" (referências circulares)
// ao converter os seus Modelos para JSON.
// ===================================================================================
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Sistema de Gestão de Certificados e Vagas de Emprego API", 
        Version = "v1",
        Description = "API para gestão de usuários, certificados, cursos, empresas e vagas de emprego"
    });
});

var app = builder.Build();

// Configura o endpoint para rodar na porta 8080 (bom para containers)
app.Urls.Add("http://*:8080");

// Habilita o Swagger
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    // Rota padrão do Swagger UI (acessar a raiz do site)
    c.RoutePrefix = string.Empty;
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Gestão API V1");
});

app.UseAuthorization();

app.MapControllers();

// ===================================================================================
// CORREÇÃO (Erro 500 no POST/PUT):
// Aplica as migrations do EF Core automaticamente.
// Isto garante que o banco de dados no Azure está sempre sincronizado
// com o código da aplicação (ex: com as chaves primárias corrigidas).
// ===================================================================================
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDb>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrations. Verifique a connection string e a conectividade com o banco de dados.");
        // Não interrompe a aplicação, mas registra o erro
    }
}

app.Run();
