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
          // Garantir que o usuário esteja autenticado
        public async Task<IActionResult> CriarEquipe([FromBody] Equipe equipeRequest)
        {
            if (equipeRequest == null || string.IsNullOrEmpty(equipeRequest.NomeEquipe))
            {
                return BadRequest("O nome da equipe é obrigatório.");
            }

            // Criar a nova equipe
            var equipe = new Equipe
            {
                NomeEquipe = equipeRequest.NomeEquipe,
                Modalidade = equipeRequest.Modalidade
            };

            _context.Equipes.Add(equipe);
            await _context.SaveChangesAsync();

            // Obter o ID do usuário que fez a requisição
            var usuarioId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (usuarioId != null)
            {
                // Criar a associação no controlador de Pertence
                var pertence = new Pertence
                {
                    CodUsuario = int.Parse(usuarioId),
                    CodEquipe = equipe.CodEquipe
                };

                _context.Pertences.Add(pertence);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(BuscarEquipePorNome), new { nome = equipe.NomeEquipe }, equipe);
        }

        // Endpoint para buscar uma equipe pelo nome
        [HttpGet("buscar/{nome}")]
        public async Task<IActionResult> BuscarEquipePorNome(string nome)
        {
            var equipe = await _context.Equipes
                .Include(e => e.Pertences)
                .ThenInclude(p => p.Usuario)
                .FirstOrDefaultAsync(e => e.NomeEquipe.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (equipe == null)
            {
                return NotFound($"Equipe com o nome '{nome}' não encontrada.");
            }

            return Ok(equipe);
        }

        // Endpoint para deletar uma equipe pelo nome
        [HttpDelete("deletar/{nome}")]
        // Garantir que o usuário esteja autenticado
        public async Task<IActionResult> DeletarEquipePorNome(string nome)
        {
            var equipe = await _context.Equipes
                .FirstOrDefaultAsync(e => e.NomeEquipe.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (equipe == null)
            {
                return NotFound($"Equipe com o nome '{nome}' não encontrada.");
            }

            // Remover associações da equipe na tabela Pertence
            var pertences = _context.Pertences.Where(p => p.CodEquipe == equipe.CodEquipe);
            _context.Pertences.RemoveRange(pertences);

            // Remover a equipe
            _context.Equipes.Remove(equipe);
            await _context.SaveChangesAsync();

            return Ok($"Equipe com o nome '{nome}' foi deletada com sucesso.");
        }
        [HttpGet("minhasEquipes")]
        [Authorize] // Garantir que o usuário esteja autenticado
        public async Task<IActionResult> ListarEquipesDoUsuario()
        {
            // Obter o ID do usuário que fez a requisição
            var usuarioId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (usuarioId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            int id = int.Parse(usuarioId);

            // Buscar equipes associadas ao usuário na tabela Pertence
            var equipes = await _context.Pertences
                .Where(p => p.CodUsuario == id)
                .Include(p => p.Equipe)
                .Select(p => p.Equipe)
                .ToListAsync();

            return Ok(equipes);
        }
    }
}
