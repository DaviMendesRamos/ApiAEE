using apiAEE.Context;
using apiAEE.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura a aplicação para autenticar os usuários usando tokens JWT,
// verificando o emissor, audiência, tempo de vida e chave de assinatura
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // Define o emissor e a audiência válidos para o token
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            // Define a chave de assinatura usada para assinar e
            // verificar o token JWT
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiAEE", Version = "v1" });

    // Define um esquema seguro para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization usando o Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Implementa a autenticação em todos os endpoints da API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Permite injetar a instância do contexto nos controladores
builder.Services.AddDbContext<AppDbContext>(option =>
                                            option.UseSqlServer(connection));

// Adiciona os serviços ao container.
builder.Services.AddControllers();

// Configura o acesso às configurações dos administradores
builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("Admins"));

// Adiciona o serviço de arquivos estáticos (acesso a pastas específicas)
builder.Services.AddDirectoryBrowser();
builder.Logging.AddDebug();
builder.Logging.AddConsole();
// Configura o suporte a CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.Urls.Add("http://0.0.0.0:5053");
// Configura a interface Swagger para API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppAEE V1");
});

// Configura o pipeline do HTTP request
// Servindo arquivos estáticos, como imagens
var userImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "userimages");

// Verifica se o diretório existe, e cria se necessário
if (!Directory.Exists(userImagesPath))
{
    Directory.CreateDirectory(userImagesPath);
}
// Se necessário, configura a pasta 'userimages' como acessível diretamente
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(userImagesPath),
    RequestPath = "/userimages"
});

// Redireciona as requisições HTTP para HTTPS


// Habilita o CORS
app.UseCors("AllowAll");

app.UseAuthentication(); // Autenticação
app.UseAuthorization(); // Autorização

// Mapeia os controladores para a aplicação
app.MapControllers();

app.Run();
