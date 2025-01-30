using apiAEE.Context;
using apiAEE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiAEE.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MembroController : ControllerBase
	{
		private readonly AppDbContext _context;

		public MembroController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Membro>>> GetPertence()
		{
			return await _context.Membros
				.Include(p => p.Usuario)
				.Include(p => p.Equipe)
				.ToListAsync();
		}

		[HttpPost("inscrever")]
		public async Task<ActionResult<Membro>> InscreverUsuarioEmEquipe([FromBody] object dados)
		{
			try
			{
				// Converter o objeto para um tipo dinâmico
				dynamic dadosDinâmicos = dados;
				int usuarioId = dadosDinâmicos.UsuarioId;
				int equipeId = dadosDinâmicos.EquipeId;

				// Encontrar usuário e equipe no banco de dados
				var usuario = await _context.Usuarios.FindAsync(usuarioId);
				var equipe = await _context.Equipes.FindAsync(equipeId);

				// Verificar se o usuário ou a equipe não foram encontrados
				if (usuario == null)
				{
					return NotFound(new { Mensagem = "Usuário não encontrado" });
				}
				if (equipe == null)
				{
					return NotFound(new { Mensagem = "Equipe não encontrada" });
				}

				// Criar o relacionamento Pertence
				var pertence = new Membro
				{
					ID = usuarioId,
					CodEquipe = equipeId
				};

				// Adicionar o relacionamento no banco
				_context.Membros.Add(pertence);
				await _context.SaveChangesAsync();

				// Retornar sucesso com o objeto Pertence criado
				return CreatedAtAction(nameof(InscreverUsuarioEmEquipe), new { usuarioId = pertence.ID, equipeId = pertence.CodEquipe }, pertence);
			}
			catch (Exception ex)
			{
				// Caso ocorra algum erro ao salvar
				return StatusCode(StatusCodes.Status500InternalServerError, new { Mensagem = "Erro ao inscrever o usuário na equipe", Detalhes = ex.Message });
			}
		}


		[HttpDelete]
		public async Task<IActionResult> RemoveUsuarioFromEquipe(int usuarioId, int equipeId)
		{
			var pertence = await _context.Membros
				.FirstOrDefaultAsync(p => p.ID == usuarioId && p.CodEquipe == equipeId);

			if (pertence == null) return NotFound();

			_context.Membros.Remove(pertence);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
