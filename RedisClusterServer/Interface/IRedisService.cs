using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedisClusterServer.Interface
{
    public interface IRedisService
    {

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

    }
}
