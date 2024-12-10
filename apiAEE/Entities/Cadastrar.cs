namespace apiAEE.Entities
{
    public class Cadastrar
    {
        public int CodEquipe { get; set; } // Identificador da equipe

        public Equipe Equipe { get; set; }
        public int CodEvento { get; set; } // Identificador do evento

        public Evento Evento { get; set; }
       
    }
}
