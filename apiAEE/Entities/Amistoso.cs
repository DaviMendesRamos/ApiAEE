namespace apiAEE.Entities
{
    public class Amistoso
    {
        public int CodEquipe { get; set; }
        public Equipe Equipe { get; set; }

        public int CodPartida { get; set; } // Altere de CodUsuario para ID
        public Partida Partida { get; set; }
    }
}
