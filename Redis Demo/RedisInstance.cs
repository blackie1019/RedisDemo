using FX.Configuration.Attributes;
using System.Collections.Generic;

namespace RedisDemo
{
    public class RedisInstance
    {
        [JsonSetting]
        public string Key { get; set; }

        [JsonSetting]
        public List<RedisEndPoint> EndPoints { get; set; }

        [JsonSetting]
        public RedisConfigurationOptionSetting ConfigurationOptions { get; set; }
    }
}