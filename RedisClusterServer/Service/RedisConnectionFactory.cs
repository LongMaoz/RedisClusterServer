using RedisClusterServer.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RedisClusterServer.Service
{
    internal class RedisConnectionFactory
    {
        private static IEnumerable<RedisEntity> _model;

        public static Lazy<RedisConnectionFactory> ConnFactory = new Lazy<RedisConnectionFactory>(() =>
        {
            return new RedisConnectionFactory();
        });

        /// <summary>
        /// RDS链接信息的模型委托
        /// </summary>
        private Action<IEnumerable<RedisEntity>> _connectionDelegate = null;

        /// <summary>
        /// 添加MDB的链接信息
        /// </summary>
        /// <param name="connectionDelegate"></param>
        /// <returns></returns>
        public RedisConnectionFactory AddMongoDbConnectionInfo(Action<IEnumerable<RedisEntity>> connectionDelegate)
        {
            _connectionDelegate = delegate (IEnumerable<RedisEntity> x)
            {
                connectionDelegate(x);
                _model = x;
            };
            return ConnFactory.Value;
        }

        /// <summary>
        /// 链式调用
        /// </summary>
        public void Build()
        {
            if (_connectionDelegate != null)
            {
                _connectionDelegate.Invoke(new List<RedisEntity>() { });
            }
        }


        public IDatabase GetConnection()
        {
            var configOptions = new ConfigurationOptions
            {
                CommandMap = CommandMap.Create(new HashSet<string>
                {
                }, available: false),
                KeepAlive = 180,
                AbortOnConnectFail = false,
                Password = _model.FirstOrDefault().Password,
            };
            foreach (var x in _model)
            {
                configOptions.EndPoints.Add(x.Host, x.Port);
            }
            var connect = ConnectionMultiplexer.Connect(configOptions);
            return connect.GetDatabase();
        }

        public IDatabase GetConnection(IEnumerable<RedisEntity> entity)
        {
            var configOptions = new ConfigurationOptions
            {
                CommandMap = CommandMap.Create(new HashSet<string>
                {
                }, available: false),
                KeepAlive = 180,
                AbortOnConnectFail = false,
                Password = _model.FirstOrDefault().Password,
            };
            foreach (var x in entity)
            {
                configOptions.EndPoints.Add(x.Host, x.Port);
            }
            var connect = ConnectionMultiplexer.Connect(configOptions);
            return connect.GetDatabase();
        }
    }
}
