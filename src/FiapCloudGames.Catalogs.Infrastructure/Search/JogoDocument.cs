using System.Text.Json.Serialization;

namespace FiapCloudGames.Catalogs.Infrastructure.Search
{
    internal sealed class JogoDocument
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = string.Empty;

        [JsonPropertyName("preco")]
        public double Preco { get; set; }

        public static JogoDocument FromResponse(Application.DTOs.Jogos.JogoResponse jogo) => new()
        {
            Id = jogo.Id.ToString(),
            Titulo = jogo.Titulo,
            Descricao = jogo.Descricao,
            Preco = (double)jogo.Preco
        };
    }
}
