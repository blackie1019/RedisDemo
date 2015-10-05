namespace RedisDemo
{
    #region

    using FX.Configuration;
    using Model;
    using System;
    using System.Collections.Generic;

    #endregion

    public class RedisConfiguration : JsonConfiguration
    {
        private static readonly string FilePath = string.Format(
            "{0}{1}",
            AppDomain.CurrentDomain.BaseDirectory,
            @"\Configuration\CachingConfiguration.json");

        public RedisConfiguration()
            : base(FilePath)
        {
        }

        public List<RedisInstanceGroup> RedisInstanceGroups { get; set; }
    }
}