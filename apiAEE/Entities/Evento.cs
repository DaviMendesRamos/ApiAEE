using System.ComponentModel.DataAnnotations;
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
    }
}
