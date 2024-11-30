using apiAEE.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// configura a aplica��o para autenticar os usu�rios usando tokens JWT,
// verificando o emissor, audi�ncia, tempo de vida e chave de assinatura
// do emissor
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			//define o emissor e a audi�ncia validas para o token
			//JWT obtidos da aplica��o
			ValidAudience = builder.Configuration["JWT:Audience"],
			ValidIssuer = builder.Configuration["JWT:Issuer"],
			//Define a chave de assinatura usada para assinar e
			//verificar o token JWT.
			IssuerSigningKey = new SymmetricSecurityKey(Encoding
				.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
		};
	});

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiECommerce", Version = "v1" });

	// Define um esquema securo para JWT
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization usando o Bearer scheme",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer"
	});

	// Implementa a autentica��o em todos os endpoints da API
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

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

//permite injetar a inst�ncia do contexto nos controladores
builder.Services.AddDbContext<AppDbContext>(option =>
											option.UseSqlServer(connection));

// Adiciona os servi�os ao container.
builder.Services.AddControllers();


var app = builder.Build();

app.UseCors(options =>
	options.AllowAnyOrigin()  // Permite todos os origens (ajuste conforme necess�rio)
		   .AllowAnyMethod()
		   .AllowAnyHeader()
);
// Configurar o servidor para ouvir em 192.168.1.6:5053
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppLanches V1");
});

// Configure o pipeline do HTTP request 
app.UseStaticFiles();
app.UseHttpsRedirection();

// Ativar autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Mapear os controladores
app.MapControllers();

// Configura��o para escutar no IP da rede local (192.168.1.6) na porta 5053
app.Run("http://192.168.1.6:5053");
