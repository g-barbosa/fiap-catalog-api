using System.Text.Json;
using FiapCloudGames.Catalogs.Application.DTOs.Jogos;
using FiapCloudGames.Catalogs.Application.Jogos.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace FiapCloudGames.Catalogs.Infrastructure.Caching
{
    public class RedisJogoCache : IJogoCache
    {
        private const string ChaveTodos = "jogos:all";
        private static string ChavePorId(Guid id) => $"jogos:{id}";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IDistributedCache _cache;
        private readonly TimeSpan _ttl;

        public RedisJogoCache(IDistributedCache cache, IConfiguration configuration)
        {
            _cache = cache;
            var ttlSeconds = configuration.GetValue("Cache:JogosTtlSeconds", 300);
            _ttl = TimeSpan.FromSeconds(ttlSeconds);
        }

        public async Task<IReadOnlyList<JogoResponse>?> ObterTodosAsync()
        {
            var json = await _cache.GetStringAsync(ChaveTodos);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<List<JogoResponse>>(json, JsonOptions);
        }

        public async Task DefinirTodosAsync(IEnumerable<JogoResponse> jogos)
        {
            var json = JsonSerializer.Serialize(jogos.ToList(), JsonOptions);
            await _cache.SetStringAsync(ChaveTodos, json, CreateOptions());
        }

        public async Task<JogoResponse?> ObterPorIdAsync(Guid id)
        {
            var json = await _cache.GetStringAsync(ChavePorId(id));
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonSerializer.Deserialize<JogoResponse>(json, JsonOptions);
        }

        public async Task DefinirPorIdAsync(JogoResponse jogo)
        {
            var json = JsonSerializer.Serialize(jogo, JsonOptions);
            await _cache.SetStringAsync(ChavePorId(jogo.Id), json, CreateOptions());
        }

        public async Task InvalidarAsync(Guid? id = null)
        {
            await _cache.RemoveAsync(ChaveTodos);

            if (id.HasValue)
                await _cache.RemoveAsync(ChavePorId(id.Value));
        }

        private DistributedCacheEntryOptions CreateOptions() => new()
        {
            AbsoluteExpirationRelativeToNow = _ttl
        };
    }
}