using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
    public class Evento
    {
        [Key]
        [JsonIgnore]
        public int CodEvento { get; set; } // Identificador único do evento
        public string NomeEvento { get; set; } // Nome do evento
        public string LocalEvento { get; set; } // Local onde o evento ocorre
        public DateTime DataInicio { get; set; } // Data de início do evento
        public DateTime DataFim { get; set; } // Data de término do evento

        // Ignorar 'UrlImagem' e 'Imagem' dependendo do contexto
        [JsonIgnore]
        public string? UrlImagem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile? Imagem { get; set; }

        [JsonIgnore]
        public ICollection<Inscricao>? Inscricoes { get; set; }

        // Propriedade para associação com mídias (opcional)
        [JsonIgnore]
        public ICollection<Midia>? Midias { get; set; } // Relacionamento com mídias associadas

        // A propriedade que controla se o CodEvento será serializado
        [JsonPropertyName("CodEvento")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Ignora quando o valor for nulo
        public int? CodEventoSerializado => DeveSerializarCod ? CodEvento : (int?)null; // Serializa CodEvento apenas se DeveSerializarCod for verdadeiro

        [JsonIgnore]
        [NotMapped]
        public bool DeveSerializarCod { get; set; } = false; // Controla se deve ou não serializar o CodEvento o CodEvento
    }
}
