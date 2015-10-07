namespace RedisDemo
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Enums;

    using Models;

    using RedisDemo.Model;

    using StackExchange.Redis;

    #endregion

    public class RedisConnectionFactory
    {
        private static readonly Lazy<RedisConnectionFactory> LazyInstance = new Lazy<RedisConnectionFactory>(() => new RedisConnectionFactory());

        private RedisConnectionFactory()
        {
            this.ActiveConnectionMultiplexers = new Dictionary<string, ConnectionMultiplexer>();
            this.ConfigurationGroupList = this.PacketConfigurationGroup();
        }

        public static RedisConnectionFactory Instance
        {
            get { return LazyInstance.Value; }
        }

        private Dictionary<string, ConnectionMultiplexer> ActiveConnectionMultiplexers { get; set; }

        private List<RedisConfigurationGroup> ConfigurationGroupList { get; set; }

        public IDatabase GetDatabase(GroupTypeEnum type, string key)
        {
            ConnectionMultiplexer redis;
            if (this.ActiveConnectionMultiplexers.TryGetValue(this.ConvertMultiplexerKey(type, key), out redis)
                && redis.IsConnected)
            {
                return redis.GetDatabase();
            }

            var config = this.ConfigurationGroupList.First(g => g.Type == type).ConfigurationOptionsList.First(c => c.Key.ToLower() == key.ToLower());
            if (redis == null)
            {
                redis = ConnectionMultiplexer.Connect(config.ConfigurationOptions);
                this.ActiveConnectionMultiplexers.Add(this.ConvertMultiplexerKey(type, key), redis);
            }
            else
            {
                redis = ConnectionMultiplexer.Connect(config.ConfigurationOptions);
                this.ActiveConnectionMultiplexers[this.ConvertMultiplexerKey(type, key)] = redis;
            }

            return redis.GetDatabase();
        }

        #region private

        private string ConvertMultiplexerKey(GroupTypeEnum type, string key)
        {
            return string.Format("{0}{1}", type, key);
        }

        private List<RedisConfigurationGroup> PacketConfigurationGroup()
        {
            var result = new List<RedisConfigurationGroup>();
            var configuraion = new RedisConfiguration();

            configuraion.RedisInstanceGroups.ForEach(g => { result.Add(this.ConstructConfigurationGroup(g)); });

            return result;
        }

        private RedisConfigurationGroup ConstructConfigurationGroup(RedisInstanceGroup instanceGroup)
        {
            var groupType = (GroupTypeEnum)Enum.Parse(typeof(GroupTypeEnum), instanceGroup.Type);
            var result = new RedisConfigurationGroup(groupType);
            foreach (var instance in instanceGroup.EndPoints)
            {
                var redisOption = new RedisConfigurationOption
                {
                    Key = instance.Name,
                    ConfigurationOptions = ConfigurationOptions.Parse(instance.ConnectionSetting)
                };
                result.ConfigurationOptionsList.Add(redisOption);
            }

            return result;
        }

        #endregion
    }
}