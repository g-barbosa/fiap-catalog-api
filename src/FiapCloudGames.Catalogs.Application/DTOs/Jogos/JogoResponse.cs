namespace FiapCloudGames.Catalogs.Application.DTOs.Jogos
{
    public class JogoResponse
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}