using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;

namespace RedisDemo.Test
{
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
            var redis = manager.CreateConnection(redisKey);
            //var redis = ConnectionMultiplexer.Connect("192.168.100.99:32768");
            var db = redis.GetDatabase();

            db.StringSet(testKey, testValue);
            var expected = testValue;
            var actual = db.StringGet(testKey);

            Assert.AreEqual(expected, actual);

        }
    }
}
