namespace apiAEE.Entities
{
    public class Membro
    {
        public int CodEquipe { get; set; }
        public Equipe Equipe { get; set; }

        public int ID { get; set; } // Altere de CodUsuario para ID
        public Usuario Usuario { get; set; }
    }
}