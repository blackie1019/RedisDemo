namespace RedisDemo.Model
{
    using Enums;
    #region

    using System.Collections.Generic;

    #endregion

    public class RedisConfigurationGroup
    {
        public RedisConfigurationGroup(GroupTypeEnum type)
        {
            this.Type = type;
            this.ConfigurationOptionsList = new List<RedisConfigurationOption>();
        }

        public GroupTypeEnum Type{ get; set; }

        public List<RedisConfigurationOption> ConfigurationOptionsList { get; set; }
    }
}