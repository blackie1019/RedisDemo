using FX.Configuration;
using FX.Configuration.Attributes;
using System.Collections.Generic;

namespace RedisDemo
{
    public class RedisConfiguration : JsonConfiguration
    {
        private static string FilePath = string.Format("{0}{1}", System.AppDomain.CurrentDomain.BaseDirectory, @"\Configuration\RedisConfiguration.json");

        public RedisConfiguration() : base(FilePath)
        {

        }

        [JsonSetting]
        public List<RedisInstance> RedisInstances { get; set; }

    }
}