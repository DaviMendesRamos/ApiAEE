using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
    public class Cadastrar
    {
        public int CodEquipe { get; set; } // Identificador da equipe
        [JsonIgnore]
        public Equipe? Equipe { get; set; }
        public int CodEvento { get; set; } // Identificador do evento

        [JsonIgnore]
        public Evento? Evento { get; set; }

        [JsonIgnore]
        public ICollection<Participar>? Participar { get; set; }

    }
}
