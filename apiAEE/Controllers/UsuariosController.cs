using apiAEE.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiAEE.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
	private readonly AppDbContext dbContext;
	private readonly IConfiguration _config;

	public UsuariosController(AppDbContext dbContext, IConfiguration config)
	{
		_config = config;
		this.dbContext = dbContext;
	}

	[HttpPost("[action]")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]


    public async Task<IActionResult> Register([FromBody] Usuario usuario)
    {
        try
        {
            // Verifica se o usuário já existe
            var usuarioExiste = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (usuarioExiste is not null)
            {
                return BadRequest("Já existe usuário com este email");
            }

            // Criação de um hash para a senha usando PasswordHasher
            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.Senha = passwordHasher.HashPassword(usuario, usuario.Senha);

            // Adiciona o usuário ao banco de dados
            dbContext.Usuarios.Add(usuario);
            await dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            // Log de erro detalhado
            Console.WriteLine($"Erro: {ex.Message}");

            // Retorna um erro 500 com detalhes do problema
            return StatusCode(StatusCodes.Status500InternalServerError, new { Mensagem = "Erro ao processar o registro", Detalhes = ex.Message });
        }
    }




    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] Usuario usuario)
    {
        try
        {
            // Verifica se o usuário existe
            var usuarioAtual = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (usuarioAtual is null)
            {
                return NotFound("Usuário não encontrado");
            }

            // Verifica se a senha fornecida corresponde ao hash da senha armazenada
            var passwordHasher = new PasswordHasher<Usuario>();
            var senhaValida = passwordHasher.VerifyHashedPassword(usuarioAtual, usuarioAtual.Senha, usuario.Senha);

            if (senhaValida != PasswordVerificationResult.Success)
            {
                return Unauthorized("Senha incorreta");
            }

            // Verifica se o usuário é admin (baseado na configuração)
            var adminEmails = _config.GetSection("Admins").Get<List<string>>();
            bool isAdmin = adminEmails.Contains(usuarioAtual.Email);

            // Criação do token JWT
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, usuario.Email!),
            new Claim(ClaimTypes.Name, usuarioAtual.Nome),
            new Claim(ClaimTypes.Role, isAdmin ? "admin" : "user") // Role adicionada ao token
        };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(5),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                accesstoken = jwt,
                expiration = token.ValidTo,
                tokentype = "bearer",
                usuarioid = usuarioAtual.Id,
                usuarionome = usuarioAtual.Nome,
                role = isAdmin ? "admin" : "user"
            });
        }
        catch (Exception ex)
        {
            // Retorna erro detalhado para facilitar depuração
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Mensagem = "Erro ao processar o login",
                Detalhes = ex.Message
            });
        }
    }

}






