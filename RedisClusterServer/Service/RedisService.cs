using RedisClusterServer.Interface;
using RedisClusterServer.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace RedisClusterServer.Service
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;

        /// <summary>
        /// //注入或实例化之前先在启动文件中调用(不在此类库中调用)
        /// RedisConnectionFactory.ConnFactory.Value.AddMongoDbConnectionInfo(x =>{}).Build();
        /// </summary>
        public RedisService() {
            
            _database = RedisConnectionFactory.ConnFactory.Value.GetConnection();
        }

        public RedisService(IEnumerable<RedisEntity> entities)
        {
            _database = RedisConnectionFactory.ConnFactory.Value.GetConnection(entities);
        }

        public async Task<IEnumerable<T>> HashGetAllAsync<T>(string hashKey) where T : class
        {
            HashEntry[] hashEntries =  await _database.HashGetAllAsync(hashKey, CommandFlags.PreferReplica);
            return hashEntries.Select(x => JsonConvert.DeserializeObject<T>(x.Value));
        }

        public async Task<IEnumerable<T>> HashGetAllParallelAsync<T>(string hashKey) where T : class
        {
            HashEntry[] hashEntries = await _database.HashGetAllAsync(hashKey, CommandFlags.PreferReplica);
            return hashEntries.AsParallel().Select(x => JsonConvert.DeserializeObject<T>(x.Value));
        }

        public async Task<HashEntry[]> HashGetAllAsync(string hashKey)
        {
            return await _database.HashGetAllAsync(hashKey, CommandFlags.PreferReplica);
        }

    }
}
