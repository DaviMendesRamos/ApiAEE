using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
    public class Equipe
    {
        [JsonIgnore]
        [Key]
        public int CodEquipes { get; set; }

        public string NomeEquipe { get; set; } = string.Empty;
        public string? NomeJogadores { get; set; } = string.Empty;

        // Relacionamento com "Pertence"
        [JsonIgnore]
        public ICollection<Pertence>? Pertences { get; set; }

        // Relacionamento com o criador
        public int CriadorId { get; set; }  // Referência para o usuário criador
        public Usuario Criador { get; set; }  // Relacionamento com o usuário criador
    }
}
