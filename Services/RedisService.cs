using StackExchange.Redis;

namespace WebApplication1.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService(string redisHost)
        {
            _redis = ConnectionMultiplexer.Connect(redisHost);
            _db = _redis.GetDatabase();
        }

        public async Task SetStringAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }
    }
}

