namespace RedisDemo
{
    using Model;
    using Enums;

    #region

    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class RedisManager
    {
        private static readonly Lazy<RedisManager> lazyInstance = new Lazy<RedisManager>(() => new RedisManager());
        private Dictionary<string,ConnectionMultiplexer> activeConnectionMultiplexers { get; }
        private List<RedisConfigurationGroup> configurationGroupList { get; }

        private RedisManager()
        {
            this.activeConnectionMultiplexers = new Dictionary<string, ConnectionMultiplexer>();
            this.configurationGroupList = this.PacketConfigurationGroup();
        }

        public static RedisManager Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        public IDatabase GetDatabase(GroupTypeEnum type,string key)
        {
            ConnectionMultiplexer redis;
            if (activeConnectionMultiplexers.TryGetValue(ConvertMultiplexerKey(type, key), out redis) && redis.IsConnected)
            {

            }
            else
            {
                var config = this.configurationGroupList.First(g => g.Type == type).ConfigurationOptionsList.First(c => c.Key.ToLower() == key.ToLower());
                if (redis == null)
                {
                    redis = ConnectionMultiplexer.Connect(config.ConfigurationOptions);
                    activeConnectionMultiplexers.Add(ConvertMultiplexerKey(type, key), redis);
                }
                else
                {
                    redis = ConnectionMultiplexer.Connect(config.ConfigurationOptions);
                    activeConnectionMultiplexers[ConvertMultiplexerKey(type, key)] = redis;
                }                
            }
            return redis.GetDatabase();
        }

        #region private

        private string ConvertMultiplexerKey(GroupTypeEnum type, string key)
        {
            return type + key;
        }

        private List<RedisConfigurationGroup> PacketConfigurationGroup()
        {
            var result = new List<RedisConfigurationGroup>();
            var configuraion =new RedisConfiguration();

            configuraion.RedisInstanceGroups.ForEach(g => result.Add(ConstructConfigurationGroup(g)));

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