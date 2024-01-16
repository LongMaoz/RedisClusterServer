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
using Newtonsoft.Json.Linq;

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

        #region Hash

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

        public async Task<bool> HashSetAsync<T>(string hashkey,string key,T value) where T : class
        {
            string json = JsonConvert.SerializeObject(value);
            return await _database.HashSetAsync(hashkey,key,json);
        }

        public async Task HashSetAsync(string hashkey, HashEntry[] entries)
        {
            await _database.HashSetAsync(hashkey, entries);
        }

        #endregion

        #region String

        public async Task<bool> StringSetAsync(string key, string val)
        {
            return await _database.StringSetAsync(key,val);
        }

        public async Task<bool> StringSetAsync(Dictionary<string, string> dic)
        {
            var pairs = dic.Select(x => new KeyValuePair<RedisKey, RedisValue>(x.Key, x.Value)).ToArray();
            return await _database.StringSetAsync(pairs);
        }

        public async Task<bool> StringSetParalleAsync(ConcurrentDictionary<string, string> dic)
        {
            var pairs = dic.AsParallel().Select(x => new KeyValuePair<RedisKey, RedisValue>(x.Key, x.Value)).ToArray();
            return await _database.StringSetAsync(pairs);
        }

        public async Task<T> StringGetAsync<T>(string key)
        {
            string val = await _database.StringGetAsync(key,CommandFlags.PreferReplica);
            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(val, typeof(T));
            return JsonConvert.DeserializeObject<T>(val);
        }

        #endregion

    }
}
