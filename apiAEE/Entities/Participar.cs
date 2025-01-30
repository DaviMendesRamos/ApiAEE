namespace apiAEE.Entities
{
    public class Participar
    {
        public int CodEquipe { get; set; } // Chave estrangeira para Cadastrar (Equipe)
        public int CodEvento { get; set; } // Chave estrangeira para Cadastrar (Evento)

        public Inscricao Inscricao { get; set; } // Propriedade de navegação para Cadastrar

        public int CodPartida { get; set; } // Chave estrangeira para Partida
        public Partida Partida { get; set; } // Propriedade de navegação para Partida
    }
}
