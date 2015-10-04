namespace RedisDemo
{
    #region

    using System.Collections.Generic;

    using FX.Configuration.Attributes;

    #endregion

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