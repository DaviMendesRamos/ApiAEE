using Microsoft.AspNetCore.Mvc;
using apiAEE.Entities;
using Microsoft.EntityFrameworkCore;
using apiAEE.Context;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public async Task<IActionResult> CriarEquipe([FromBody] Equipe equipeRequest)
        {
            if (equipeRequest == null || string.IsNullOrEmpty(equipeRequest.NomeEquipe))
            {
                return BadRequest("O nome da equipe é obrigatório.");
            }

            // Obter o ID do usuário do JWT
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (usuarioId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Criar a nova equipe
            var equipe = new Equipe
            {
                NomeEquipe = equipeRequest.NomeEquipe,
                Modalidade = equipeRequest.Modalidade
            };

            _context.Equipes.Add(equipe);
            await _context.SaveChangesAsync();

            // Criar a associação no controlador de Pertence
            var pertence = new Pertence
            {
                ID = int.Parse(usuarioId),
                CodEquipe = equipe.CodEquipe
            };

            _context.Pertences.Add(pertence);
            await _context.SaveChangesAsync();

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
        [HttpGet("equipesDoUsuario")]
        // Garante que apenas usuários autenticados possam acessar

        public async Task<IActionResult> ListarEquipesDoUsuario()
        {
            // Obtém o ID do usuário do token JWT
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Converte o ID para int (ou o tipo que você está usando para IDs)
            var id = int.Parse(usuarioId);

            // Busca as equipes associadas ao usuário
            var equipes = await _context.Pertences
                .Where(p => p.ID == id)
                .Select(p => p.Equipe) // Retorna apenas as equipes associadas
                .ToListAsync();

            if (equipes == null || equipes.Count == 0)
            {
                return Ok(new List<Equipe>());
            }

            // Define a propriedade `DeveSerializarCod` para cada equipe
            foreach (var equipe in equipes)
            {
                equipe.DeveSerializarCod = true; // Define como true para serializar CodEquipe
            }

            return Ok(equipes); // Retorna as equipes com CodEquipe configurado
        }

    }
}
