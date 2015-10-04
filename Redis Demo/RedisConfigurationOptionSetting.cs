namespace RedisDemo
{
    public class RedisConfigurationOptionSetting
    {
        public bool AbortOnConnectFail { get; set; }

        public bool AllowAdmin { get; set; }

        public string ChannelPrefix { get; set; }

        public string ClientName { get; set; }

        public string ConfigurationChannel { get; set; }

        public int ConnectRetry { get; set; }

        public int ConnectTimeout { get; set; }

        public int DefaultDatabase { get; set; }

        public string DefaultVersion { get; set; }

        public int KeepAlive { get; set; }

        public string Password { get; set; }

        public string Proxy { get; set; }

        public bool ResolveDns { get; set; }

        public string ServiceName { get; set; }

        public bool Ssl { get; set; }

        public string SslHost { get; set; }

        public int SyncTimeout { get; set; }

        public string TieBreaker { get; set; }

        public int WriteBuffer { get; set; }
    }
}