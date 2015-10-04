namespace RedisDemo.Test
{
    #region

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Enums;
    using StackExchange.Redis;

    #endregion

    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void SetGet_Helloworld_String()
        {
            var redisKey = "redis01";
            var type = GroupTypeEnum.Cache;
            var testKey = "testkey";
            var testValue = "Hello World!!";
            var manager = RedisManager.Instance;

            var db = manager.GetDatabase(type, redisKey);

            db.StringSet(testKey, testValue);
            var expected = testValue;
            var actual = db.StringGet(testKey).ToString();

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void SetGet_Helloworld_String_use_singleton()
        {
            var redisKey = "redis01";
            var type = GroupTypeEnum.Cache;
            var testKey = "testkey";
            var testValue = "Hello World!!";

            var testKey2 = "testkey2";
            var testValue2 = "Hello World2!!";

            var manager = RedisManager.Instance;


            manager.GetDatabase(type, redisKey).StringSet(testKey, testValue);
            var expected = testValue;
            var actual = manager.GetDatabase(type, redisKey).StringGet(testKey).ToString();

            Assert.AreEqual(expected, actual);

            manager.GetDatabase(type, redisKey).StringSet(testKey2, testValue2);
            var expected2 = testValue2;
            var actual2 = manager.GetDatabase(type, redisKey).StringGet(testKey2).ToString();

            Assert.AreEqual(expected2, actual2);
        }
    }
}