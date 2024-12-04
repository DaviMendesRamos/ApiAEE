using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiAEE.Context;
using apiAEE.Entities;
using System.Threading.Tasks;

namespace apiAEE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Garante que apenas usuários autenticados possam acessar
    public class EventosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        // Método para criar um novo evento, só acessível para administradores
        [HttpPost("criar")]
        [Authorize(Roles = "admin")] // Garantir que apenas administradores possam criar
        public async Task<IActionResult> CriarEvento([FromBody] Evento eventoRequest)
        {
            if (eventoRequest == null || string.IsNullOrEmpty(eventoRequest.NomeEvento))
            {
                return BadRequest("Nome do evento é obrigatório.");
            }

            var evento = new Evento
            {
                NomeEvento = eventoRequest.NomeEvento,
                LocalEvento = eventoRequest.LocalEvento,
                DataInicio = eventoRequest.DataInicio,
                DataFim = eventoRequest.DataFim
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarEventoPorId), new { id = evento.CodEvento }, evento);
        }

        // Método para editar um evento, só acessível para administradores
        [HttpPut("editar/{id}")]
        [Authorize(Roles = "admin")] // Garantir que apenas administradores possam editar
        public async Task<IActionResult> EditarEvento(int id, [FromBody] Evento eventoRequest)
        {
            if (eventoRequest == null || string.IsNullOrEmpty(eventoRequest.NomeEvento))
            {
                return BadRequest("Nome do evento é obrigatório.");
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound($"Evento com ID {id} não encontrado.");
            }

            evento.NomeEvento = eventoRequest.NomeEvento;
            evento.LocalEvento = eventoRequest.LocalEvento;
            evento.DataInicio = eventoRequest.DataInicio;
            evento.DataFim = eventoRequest.DataFim;

            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content, pois a edição foi bem-sucedida
        }

        // Método para excluir um evento, só acessível para administradores
        [HttpDelete("deletar/{id}")]
        [Authorize(Roles = "admin")] // Garantir que apenas administradores possam deletar
        public async Task<IActionResult> DeletarEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound($"Evento com ID {id} não encontrado.");
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return Ok($"Evento com ID {id} foi deletado com sucesso.");
        }

        // Método para buscar um evento pelo ID
        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> BuscarEventoPorId(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound($"Evento com ID {id} não encontrado.");
            }

            return Ok(evento);
        }

        // Método para listar todos os eventos
        [HttpGet("listar")]
        public async Task<IActionResult> ListarEventos()
        {
            try
            {
                var eventos = await _context.Eventos.ToListAsync(); // Busca todos os eventos
                return Ok(eventos); // Retorna todos os eventos
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
