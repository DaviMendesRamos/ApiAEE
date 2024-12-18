using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
    public class Midia
    {
        [JsonIgnore]
        [Key]
        public int CodMidia { get; set; }
        public string Nome { get; set; }
        
        [JsonIgnore]
        public string? UrlImagem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile? Imagem { get; set; }

        [ForeignKey("Evento")]
        public int? CodEvento { get; set; } // Relacionamento opcional com Evento

        [JsonIgnore]
        public Evento? Evento { get; set; } // Propriedade de navegação para Evento

    }
}
