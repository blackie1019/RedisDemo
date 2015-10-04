namespace RedisDemo
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StackExchange.Redis;

    #endregion

    public class RedisManager
    {
        private static readonly Lazy<RedisManager> lazyInstance = new Lazy<RedisManager>(() => new RedisManager());

        private RedisManager()
        {
            this.Configuration = new RedisConfiguration();
            this.ConfigurationOptionsList = this.ConvertToConfigurationOptions();
        }

        public static RedisManager Instance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        private RedisConfiguration Configuration { get; }

        private List<RedisConfigurationOption> ConfigurationOptionsList { get; }

        public ConnectionMultiplexer CreateConnection(string key)
        {
            var config = this.ConfigurationOptionsList.First(x => x.Key == key);
            return ConnectionMultiplexer.Connect(config.ConfigurationOptions);
        }

        #region private

        private List<RedisConfigurationOption> ConvertToConfigurationOptions()
        {
            var result = new List<RedisConfigurationOption>();

            foreach (var instance in this.Configuration.RedisInstances)
            {
                var redisOption = new RedisConfigurationOption
                                      {
                                          Key = instance.Key, 
                                          ConfigurationOptions =
                                              new ConfigurationOptions
                                                  {
                                                      AbortOnConnectFail =
                                                          instance
                                                          .ConfigurationOptions
                                                          .AbortOnConnectFail, 
                                                      AllowAdmin =
                                                          instance
                                                          .ConfigurationOptions
                                                          .AllowAdmin, 
                                                      ChannelPrefix =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ChannelPrefix, 
                                                      ClientName =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ClientName, 
                                                      ConfigurationChannel =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ConfigurationChannel, 
                                                      ConnectRetry =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ConnectRetry, 
                                                      ConnectTimeout =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ConnectTimeout, 
                                                      DefaultDatabase =
                                                          instance
                                                          .ConfigurationOptions
                                                          .DefaultDatabase, 
                                                      DefaultVersion =
                                                          string
                                                              .IsNullOrEmpty
                                                              (
                                                                  instance
                                                              .ConfigurationOptions
                                                              .Proxy)
                                                              ? null
                                                              : new Version(
                                                                    instance
                                                                    .ConfigurationOptions
                                                                    .DefaultVersion), 
                                                      KeepAlive =
                                                          instance
                                                          .ConfigurationOptions
                                                          .KeepAlive, 
                                                      Password =
                                                          instance
                                                          .ConfigurationOptions
                                                          .Password, 
                                                      ResolveDns =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ResolveDns, 
                                                      ServiceName =
                                                          instance
                                                          .ConfigurationOptions
                                                          .ServiceName, 
                                                      Ssl =
                                                          instance
                                                          .ConfigurationOptions
                                                          .Ssl, 
                                                      SslHost =
                                                          instance
                                                          .ConfigurationOptions
                                                          .SslHost, 
                                                      SyncTimeout =
                                                          instance
                                                          .ConfigurationOptions
                                                          .SyncTimeout, 
                                                      TieBreaker =
                                                          instance
                                                          .ConfigurationOptions
                                                          .TieBreaker, 
                                                      WriteBuffer =
                                                          instance
                                                          .ConfigurationOptions
                                                          .WriteBuffer
                                                  }
                                      };

                if (!string.IsNullOrEmpty(instance.ConfigurationOptions.Proxy))
                {
                    redisOption.ConfigurationOptions.Proxy = Proxy.Twemproxy;
                }

                this.UpdateEndPoints(instance.EndPoints, ref redisOption);
                result.Add(redisOption);
            }

            return result;
        }

        private void UpdateEndPoints(List<RedisEndPoint> endPoints, ref RedisConfigurationOption redisOption)
        {
            foreach (var endPoint in endPoints)
            {
                redisOption.ConfigurationOptions.EndPoints.Add(
                    string.Format("{{{0},{1}}}", endPoint.Name, endPoint.Port));
            }
        }

        #endregion
    }
}