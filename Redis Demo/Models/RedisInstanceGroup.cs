namespace RedisDemo.Models
{
    #region

    using System.Collections.Generic;

    #endregion

    public class RedisInstanceGroup
    {
        public string Type { get; set; }

        public List<RedisEndPoint> EndPoints { get; set; }
    }
}