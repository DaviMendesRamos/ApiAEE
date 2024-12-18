using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace apiAEE.Entities
{
	public class Partida
	{

		[Key]
		[JsonIgnore]
		public int CodPartida { get; set; }

		public int Pontuacao { get; set; }
		
		public int QuantMinJogadores { get; set; }

		public DateTime HoratioPartida { get; set; }

        public ICollection<Participar>? Participar { get; set; }

		public ICollection<Amistoso>? Amistoso { get; set; }
    }
}
