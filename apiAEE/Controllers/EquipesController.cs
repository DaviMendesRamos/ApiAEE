using Microsoft.AspNetCore.Mvc;
using apiAEE.Entities;
using Microsoft.EntityFrameworkCore;
using apiAEE.Context;
using Microsoft.AspNetCore.Authorization;

namespace apiAEE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EquipeController : ControllerBase
	{
		private readonly AppDbContext _context;

		public EquipeController(AppDbContext context)
		{
			_context = context;
		}

        // Endpoint para criar uma nova equipe
        [HttpPost("criarEquipe")]
        [Authorize]  // Garantir que o usuário esteja autenticado
        public async Task<IActionResult> CriarEquipe([FromBody] Equipe equipeRequest)
        {
            if (equipeRequest == null || string.IsNullOrEmpty(equipeRequest.NomeEquipe))
            {
                return BadRequest("Nome da equipe e jogadores são obrigatórios.");
            }

            // Pegar o ID do criador do token JWT
            var criadorId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (criadorId == null)
            {
                return Unauthorized("Usuário não autorizado.");
            }

            // Agora você pode associar o criador à nova equipe
            var equipe = new Equipe
            {
                NomeEquipe = equipeRequest.NomeEquipe,
                NomeJogadores = equipeRequest.NomeJogadores,
                CriadorId = int.Parse(criadorId)  // Convertendo o ID do criador para inteiro
            };

            _context.Equipes.Add(equipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarEquipePorNome), new { nome = equipe.NomeEquipe }, equipe);
        }

        // Endpoint para buscar uma equipe pelo nome
        [HttpGet("buscar/{nome}")]
		public async Task<IActionResult> BuscarEquipePorNome(string nome)
		{
			var equipe = await _context.Equipes
				.FirstOrDefaultAsync(e => e.NomeEquipe.Equals(nome, StringComparison.OrdinalIgnoreCase));

			if (equipe == null)
			{
				return NotFound($"Equipe com o nome '{nome}' não encontrada.");
			}

			return Ok(equipe);
		}

		// Endpoint para deletar uma equipe pelo nome
		[HttpDelete("deletar/{nome}")]
		public async Task<IActionResult> DeletarEquipePorNome(string nome)
		{
			var equipe = await _context.Equipes
				.FirstOrDefaultAsync(e => e.NomeEquipe.Equals(nome, StringComparison.OrdinalIgnoreCase));

			if (equipe == null)
			{
				return NotFound($"Equipe com o nome '{nome}' não encontrada.");
			}

			_context.Equipes.Remove(equipe);
			await _context.SaveChangesAsync();

			return Ok($"Equipe com o nome '{nome}' foi deletada com sucesso.");
		}
	}
}
