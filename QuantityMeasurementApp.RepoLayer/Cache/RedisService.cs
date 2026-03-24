using StackExchange.Redis;

namespace QuantityMeasurementApp.RepoLayer.Cache
{
    public class RedisService
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _connection;

        public RedisService(IConnectionMultiplexer connection)
        {
            _connection = connection;
            _db         = connection.GetDatabase();
        }

        // ── Token blacklist ───────────────────────────────────────────────────

        /// <summary>
        /// Blacklist a JWT token until its expiry time.
        /// </summary>
        public async Task BlacklistTokenAsync(string token, TimeSpan expiry)
        {
            string key = $"blacklist:{token}";
            await _db.StringSetAsync(key, "revoked", expiry);
        }

        /// <summary>
        /// Check if a JWT token has been blacklisted.
        /// </summary>
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            string key = $"blacklist:{token}";
            return await _db.KeyExistsAsync(key);
        }

        // ── General cache ─────────────────────────────────────────────────────

        /// <summary>Set a string value in cache with expiry.</summary>
        public async Task SetAsync(string key, string value, TimeSpan expiry) =>
            await _db.StringSetAsync(key, value, expiry);

        /// <summary>Get a string value from cache.</summary>
        public async Task<string?> GetAsync(string key)
        {
            RedisValue value = await _db.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        /// <summary>Delete a key from cache.</summary>
        public async Task DeleteAsync(string key) =>
            await _db.KeyDeleteAsync(key);

        /// <summary>Check if a key exists in cache.</summary>
        public async Task<bool> ExistsAsync(string key) =>
            await _db.KeyExistsAsync(key);

        /// <summary>Check if Redis is reachable.</summary>
        public bool IsConnected => _connection.IsConnected;
    }
}