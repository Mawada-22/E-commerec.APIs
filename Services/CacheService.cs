using ServicesAbstractions;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connection;

        // Camel-case so cached JSON matches the API's normal response casing.
        private static readonly JsonSerializerOptions SerializeOptions =
            new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public CacheService(IConnectionMultiplexer connection)
        {
            _connection = connection;
            _database = connection.GetDatabase();
        }

        public async Task<string?> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);
            return cachedResponse.IsNullOrEmpty ? null : cachedResponse.ToString();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return;

            var serialized = JsonSerializer.Serialize(response, SerializeOptions);
            await _database.StringSetAsync(cacheKey, serialized, timeToLive);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            // SCAN over each server's keyspace for matches (KEYS is O(N) and blocks;
            // the server iterator uses SCAN under the hood).
            foreach (var endpoint in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(endpoint);
                var keys = server.Keys(pattern: $"{pattern}*").ToArray();
                if (keys.Length > 0)
                    await _database.KeyDeleteAsync(keys);
            }
        }
    }
}
