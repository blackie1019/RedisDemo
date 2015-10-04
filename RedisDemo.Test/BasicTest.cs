namespace RedisDemo.Test
{
    #region

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StackExchange.Redis;

    #endregion

    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void SetGet_Helloworld_String()
        {
            var redisKey = "redis01";
            var testKey = "testkey";
            var testValue = "Hello World!!";
            var manager = RedisManager.Instance;
            //var redis = manager.CreateConnection(redisKey);

            var redis = ConnectionMultiplexer.Connect("172.16.45.34:6400");
            var db = redis.GetDatabase();

            db.StringSet(testKey, testValue);
            var expected = testValue;
            var actual = db.StringGet(testKey);

            Assert.AreEqual(expected, actual);
        }
    }
}