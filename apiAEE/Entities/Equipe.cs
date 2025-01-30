using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
    public class Equipe
    {
        
        [Key]
        [JsonIgnore]
        public int CodEquipe { get; set; }

        public string NomeEquipe { get; set; } = string.Empty;

        public string? Modalidade { get; set; }

        [JsonIgnore]

        public string? UrlImagem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile? Imagem { get; set; }

        // Relacionamento com "Pertence"
        [JsonIgnore]
        public ICollection<Membro>? Membros { get; set; }

        [JsonIgnore]
        public ICollection<Inscricao>? Inscricoes { get; set; }

        [JsonIgnore]
        public ICollection<Amistoso>? Amistoso { get; set; }

        [JsonPropertyName("CodEquipe")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Ignora quando o valor for nulo
        public int? CodEventoSerializado => DeveSerializarCod ? CodEquipe : (int?)null; // Serializa CodEvento apenas se DeveSerializarCod for verdadeiro

        [JsonIgnore]
        [NotMapped]
        public bool DeveSerializarCod { get; set; } = false; // Controla se deve ou não serializar o CodEvento o CodEvento


    }
}
