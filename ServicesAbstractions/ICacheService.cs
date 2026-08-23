using System;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    // Response-cache backed by Redis. Keys are the full request path+query, values
    // are the JSON-serialized response bodies.
    public interface ICacheService
    {
        Task<string?> GetCachedResponseAsync(string cacheKey);
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        // Invalidate every cached entry whose key matches a pattern (e.g. all
        // "/products*" entries after a product is added/updated).
        Task RemoveByPatternAsync(string pattern);
    }
}
