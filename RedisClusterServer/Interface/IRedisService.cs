using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisClusterServer.Interface
{
    public interface IRedisService
    {

        #region Hash

        /// <summary>
        /// 获取Hash
        /// </summary>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public Task<HashEntry[]> HashGetAllAsync(string hashKey);

        /// <summary>
        /// 获取Hash 转泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> HashGetAllAsync<T>(string hashKey) where T : class;

        /// <summary>
        /// 获取Hash 转泛型 并行处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> HashGetAllParallelAsync<T>(string hashKey) where T : class;

        /// <summary>
        /// HashSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashkey"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task<bool> HashSetAsync<T>(string hashkey, string key, T value) where T : class;


        /// <summary>
        /// HashSet 批量插入
        /// </summary>
        /// <param name="hashkey"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public Task HashSetAsync(string hashkey, HashEntry[] entries);

        #endregion 

        #region String

        /// <summary>
        /// 单独插入StringKey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Task<bool> StringSetAsync(string key, string val);

        /// <summary>
        /// 批量插入StringKey 批量操作需要指定Hash Tag
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public Task<bool> StringSetAsync(Dictionary<string, string> dic);

        /// <summary>
        /// 批量并行插入StringKey 批量操作需要指定Hash Tag
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Task<bool> StringSetParalleAsync(ConcurrentDictionary<string, string> dic);

        /// <summary>
        /// 获取String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> StringGetAsync<T>(string key);


        #endregion
    }
}
