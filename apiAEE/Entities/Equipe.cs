using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
    public class Equipe
    {
        [JsonIgnore]
        [Key]
        public int CodEquipe { get; set; }

        public string NomeEquipe { get; set; } = string.Empty;

        public string? Modalidade { get; set; }

        // Relacionamento com "Pertence"
        [JsonIgnore]
        public ICollection<Pertence>? Pertences { get; set; }

       
       
    }
}
