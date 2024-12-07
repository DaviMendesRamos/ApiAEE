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
            new Claim(ClaimTypes.Role, isAdmin ? "admin" : "user"),
            new Claim(ClaimTypes.NameIdentifier, usuarioAtual.Id.ToString()),
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

    [HttpGet("[action]")]
    public async Task<IActionResult> GetUsuarioAtual()
    {
        try
        {
            // Obtém o ID do usuário a partir do token JWT
            var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verifica se o ID do usuário existe no token
            if (string.IsNullOrEmpty(usuarioIdString))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                return BadRequest("ID de usuário inválido.");
            }

            // Busca o usuário no banco de dados
            var usuario = await dbContext.Usuarios
                .Where(u => u.Id == usuarioId)
                .Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Email,
                    u.IsAdmin // Supondo que este campo existe na tabela
                })
                .FirstOrDefaultAsync();

            // Verifica se o usuário foi encontrado
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            // Log de erro detalhado
            Console.WriteLine($"Erro: {ex.Message}");

            // Retorna um erro 500 com detalhes do problema
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Mensagem = "Erro ao buscar o usuário atual",
                Detalhes = ex.Message
            });
        }
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> EditarUsuario([FromBody] Usuario usuarioAtualizado)
    {
        try
        {
            // Obtém o ID do usuário a partir do token JWT
            var usuarioIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verifica se o ID do usuário existe no token
            if (string.IsNullOrEmpty(usuarioIdString))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Converte o ID de string para int
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                return BadRequest("ID de usuário inválido.");
            }

            // Busca o usuário no banco de dados
            var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);

            // Verifica se o usuário foi encontrado
            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Atualiza os dados do usuário apenas se os valores novos forem fornecidos
            usuario.Nome = string.IsNullOrEmpty(usuarioAtualizado.Nome) ? usuario.Nome : usuarioAtualizado.Nome;
            usuario.Email = string.IsNullOrEmpty(usuarioAtualizado.Email) ? usuario.Email : usuarioAtualizado.Email;

            // Atualiza a senha, se fornecida
            if (!string.IsNullOrEmpty(usuarioAtualizado.Senha))
            {
                var passwordHasher = new PasswordHasher<Usuario>();
                usuario.Senha = passwordHasher.HashPassword(usuario, usuarioAtualizado.Senha);
            }

            // Salva as mudanças no banco de dados
            dbContext.Usuarios.Update(usuario);
            await dbContext.SaveChangesAsync();

            return Ok(new { Mensagem = "Usuário atualizado com sucesso." });
        }
        catch (Exception ex)
        {
            // Log de erro detalhado
            Console.WriteLine($"Erro: {ex.Message}");

            // Retorna um erro 500 com detalhes do problema
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Mensagem = "Erro ao atualizar o usuário",
                Detalhes = ex.Message
            });
        }
    }

    [HttpPost("uploadfotousuario")]
    public async Task<IActionResult> UploadFotoUsuario(IFormFile imagem)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == userEmail);

        if (usuario is null)
        {
            return NotFound("Usuário não encontrado");
        }

        if (imagem is not null)
        {
            // Gera um nome de arquivo unico para a imagem enviada 
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imagem.FileName;

            // cria o caminho completo combinando o diretorio 
            // wwwroot/userimages com o nome do arquivo
            string filePath = Path.Combine("wwwroot/userimages", uniqueFileName);

            //cria o arquivo de imagem usando o caminho completo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            // Atualiza a propriedade UrlImagem do usuário
            // com a URL da imagem enviada
            usuario.UrlImagem = "/userimages/" + uniqueFileName;

            await dbContext.SaveChangesAsync();
            return Ok("Imagem enviada com sucesso");
        }
        return BadRequest("Nenhuma imagem enviada");
    }

  
    [HttpGet("[action]")]
    public async Task<IActionResult> ImagemPerfilUsuario()
    {
        //verifica se o usuário esta autenticado
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == userEmail);

        if (usuario is null)
            return NotFound("Usuário não encontrado");

        var imagemPerfil = await dbContext.Usuarios
                                .Where(x => x.Email == userEmail)
                                .Select(x => new
                                {
                                    x.UrlImagem,
                                })
                                .SingleOrDefaultAsync();

        return Ok(imagemPerfil);
    }

    



}






