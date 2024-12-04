using apiAEE.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Usuario
{
	[JsonIgnore]
	public int Id { get; set; }

	[StringLength(100)]
	public string? Nome { get; set; }

	[StringLength(150)]
	[Required]
	public string? Email { get; set; }

	[StringLength(100)]
	[Required]
	public string? Senha { get; set; }

	[StringLength(80)]
	public string? Telefone { get; set; }

	[NotMapped]
	[JsonIgnore]
	public IFormFile? Imagem { get; set; }

	// Coleção de relacionamentos com equipes
	[JsonIgnore]
	public ICollection<Pertence> Pertences { get; set; } = new List<Pertence>();

    public bool IsAdmin { get; set; } = false;
}
