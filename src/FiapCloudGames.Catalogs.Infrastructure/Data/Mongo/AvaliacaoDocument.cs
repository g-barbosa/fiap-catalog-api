using FiapCloudGames.Catalogs.Domain.Avaliacoes.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Mongo
{
    /// <summary>
    /// Documento MongoDB da collection avaliacoes.
    /// Separado do domínio para não acoplar a entidade a atributos BSON.
    /// </summary>
    internal sealed class AvaliacaoDocument
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid JogoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }

        public static AvaliacaoDocument FromEntity(Avaliacao avaliacao) => new()
        {
            Id = avaliacao.Id,
            JogoId = avaliacao.JogoId,
            UsuarioId = avaliacao.UsuarioId,
            NomeUsuario = avaliacao.NomeUsuario,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            DataCriacao = avaliacao.DataCriacao
        };

        public Avaliacao ToEntity() => new(JogoId, UsuarioId, NomeUsuario, Nota, Comentario)
        {
            Id = Id,
            DataCriacao = DataCriacao
        };
    }
}