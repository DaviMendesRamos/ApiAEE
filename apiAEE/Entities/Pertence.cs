namespace apiAEE.Entities
{
    public class Pertence
    {
        public int CodEquipe { get; set; }
        public Equipe Equipe { get; set; }

        public int ID { get; set; } // Altere de CodUsuario para ID
        public Usuario Usuario { get; set; }
    }
}