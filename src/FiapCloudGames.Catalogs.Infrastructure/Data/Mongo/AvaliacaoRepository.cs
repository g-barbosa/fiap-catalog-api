using FiapCloudGames.Catalogs.Domain.Avaliacoes.Entities;
using FiapCloudGames.Catalogs.Domain.Avaliacoes.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FiapCloudGames.Catalogs.Infrastructure.Data.Mongo
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly IMongoCollection<AvaliacaoDocument> _collection;

        static AvaliacaoRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(AvaliacaoDocument)))
            {
                BsonClassMap.RegisterClassMap<AvaliacaoDocument>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(x => x.Id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    cm.MapMember(x => x.JogoId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                    cm.MapMember(x => x.UsuarioId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
            }
        }

        public AvaliacaoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<AvaliacaoDocument>("avaliacoes");
            var index = Builders<AvaliacaoDocument>.IndexKeys.Ascending(x => x.JogoId);
            _collection.Indexes.CreateOne(new CreateIndexModel<AvaliacaoDocument>(index));
        }

        public async Task AdicionarAsync(Avaliacao avaliacao)
        {
            await _collection.InsertOneAsync(AvaliacaoDocument.FromEntity(avaliacao));
        }

        public async Task<IReadOnlyList<Avaliacao>> ObterPorJogoIdAsync(Guid jogoId)
        {
            var documentos = await _collection
                .Find(x => x.JogoId == jogoId)
                .SortByDescending(x => x.DataCriacao)
                .ToListAsync();

            return [.. documentos.Select(x => x.ToEntity())];
        }
    }
}