

namespace apiAEE.Entities
{
    public class Pertence
    {
        public int CodEquipe { get; set; }
        public Equipe Equipe { get; set; }

        public int CodUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public bool Aceito { get; set; }  // Se o participante foi aceito na equipe
    }
}
