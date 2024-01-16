using System;
using System.Collections.Generic;
using System.Text;

namespace RedisClusterServer.Model
{
    public class RedisEntity
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }

    }
}
