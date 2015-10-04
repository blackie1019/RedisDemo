namespace RedisDemo
{
    #region

    using System;
    using System.Collections.Generic;

    using FX.Configuration;
    using FX.Configuration.Attributes;

    #endregion

    public class RedisConfiguration : JsonConfiguration
    {
        private static readonly string FilePath = string.Format(
            "{0}{1}", 
            AppDomain.CurrentDomain.BaseDirectory, 
            @"\Configuration\RedisConfiguration.json");

        public RedisConfiguration()
            : base(FilePath)
        {
        }

        [JsonSetting]
        public List<RedisInstance> RedisInstances { get; set; }
    }
}