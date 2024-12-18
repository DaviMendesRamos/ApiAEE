using Microsoft.AspNetCore.Mvc;
using apiAEE.Entities;
using apiAEE.Context;
using Microsoft.EntityFrameworkCore;

namespace apiAEE.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        public class CadastrarController : ControllerBase
        {
            private static readonly List<Cadastrar> Inscricoes = new();

            private readonly AppDbContext dbContext;
                private readonly IConfiguration _config;

        public CadastrarController(AppDbContext dbContext, IConfiguration config)
        {
            _config = config;
            this.dbContext = dbContext;
        }

        [HttpPost("InscreverEquipe")]
       
        public async Task<IActionResult> InscreverEquipe([FromBody] Cadastrar request)
        {
            if (request == null || request.CodEvento <= 0 || request.CodEquipe <= 0)
            {
                return BadRequest("Dados inválidos. Certifique-se de enviar um CodEvento e CodEquipe válidos.");
            }

            // Verifica se a equipe já está inscrita no evento
            var inscricaoExistente = await dbContext.Cadastras
                .AnyAsync(i => i.CodEvento == request.CodEvento && i.CodEquipe == request.CodEquipe);

            if (inscricaoExistente)
            {
                return Conflict("A equipe já está inscrita neste evento.");
            }

            // Cria uma nova inscrição
            var novaInscricao = new Cadastrar
            {
                CodEvento = request.CodEvento,
                CodEquipe = request.CodEquipe,
               
            };

            // Adiciona a inscrição ao banco de dados
            dbContext.Cadastras.Add(novaInscricao);
            await dbContext.SaveChangesAsync();

            return Ok("Inscrição realizada com sucesso.");
        }

        [HttpGet("{codEvento}/Equipes")]
        public async Task<IActionResult> ListarEquipesPorEvento(int codEvento)
        {
            if (codEvento <= 0)
            {
                return BadRequest("O código do evento é inválido.");
            }

            // Busca as equipes inscritas no evento
            var equipes = await dbContext.Cadastras
                .Where(c => c.CodEvento == codEvento)
                .Include(c => c.Equipe) // Faz o join com a entidade Equipe
                .Select(c => new
                {
                    c.Equipe.CodEquipe,
                    c.Equipe.NomeEquipe
                })
                .ToListAsync();

            if (equipes == null || !equipes.Any())
            {
                return NotFound("Nenhuma equipe inscrita neste evento.");
            }

            return Ok(equipes);
        }
    }


    
}
