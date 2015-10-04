using StackExchange.Redis;

namespace RedisDemo
{
    public class RedisConfigurationOption
    {
        public string Key { get; set; }
        public ConfigurationOptions ConfigurationOptions { get; set; }
    }
}
