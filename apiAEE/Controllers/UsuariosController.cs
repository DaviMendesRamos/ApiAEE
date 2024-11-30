using apiAEE.Context;
using ApiAEE.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
			var usuarioExiste = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

			if (usuarioExiste is not null)
			{
				return BadRequest("Já existe usuário com este email");
			}

			dbContext.Usuarios.Add(usuario);
			await dbContext.SaveChangesAsync();
			return StatusCode(StatusCodes.Status201Created);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status400BadRequest);
		}
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Login([FromBody] Usuario usuario)
	{
		var usuarioAtual = await dbContext.Usuarios.FirstOrDefaultAsync(u =>
								 u.Email == usuario.Email && u.Senha == usuario.Senha);

		if (usuarioAtual is null)
		{
			return NotFound("Usuário não encontrado");
		}

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.Email , usuario.Email!)
		};

		var token = new JwtSecurityToken(
			issuer: _config["JWT:Issuer"],
			audience: _config["JWT:Audience"],
			claims: claims,
			expires: DateTime.Now.AddDays(5),
			signingCredentials: credentials);

		var jwt = new JwtSecurityTokenHandler().WriteToken(token);

		return new ObjectResult(new
		{
			accesstoken = jwt,
			expiration = token.ValidTo,
			tokentype = "bearer",
			usuarioid = usuarioAtual.Id,
			usuarionome = usuarioAtual.Nome
		});
	}

	

	
}
