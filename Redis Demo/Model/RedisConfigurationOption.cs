namespace RedisDemo.Model
{
    #region

    using StackExchange.Redis;

    #endregion

    public class RedisConfigurationOption
    {
        public string Key { get; set; }

        public ConfigurationOptions ConfigurationOptions { get; set; }
    }
}