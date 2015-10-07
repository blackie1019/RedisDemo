namespace RedisDemo.Test
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using RedisDemo;
    using RedisDemo.Enums;

    using StackExchange.Redis;

    #endregion

    [TestClass]
    public class RedisTest
    {
        #region Basic Type

        [TestMethod]
        public void Test_RedisString()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            redisDb.StringSet("RedisString", "ABC");
            var existed = redisDb.KeyExists("RedisString");

            Assert.AreEqual(true, existed);

            var value = redisDb.StringGet("RedisString");
            Assert.AreEqual("ABC", value.ToString());

            redisDb.KeyDelete("RedisString");
            existed = redisDb.KeyExists("RedisString");

            Assert.AreEqual(false, existed);
        }

        [TestMethod]
        public void Test_RedisList()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisList";

            redisDb.ListLeftPush(key, "1");
            redisDb.ListLeftPush(key, "2");
            redisDb.ListLeftPush(key, "3");
            redisDb.ListRightPush(key, "0");
            redisDb.ListRightPush(key, "-1");

            RedisValue result = redisDb.ListRightPop(key);
            Assert.AreEqual(-1, result);

            RedisValue result1 = redisDb.ListRightPop(key);
            Assert.AreEqual(0, result1);

            RedisValue result2 = redisDb.ListLeftPop(key);
            Assert.AreEqual(3, result2);

            redisDb.KeyDelete(key);
        }

        [TestMethod]
        public void Test_RedisSet()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key1 = "RedisSet1";

            redisDb.SetAdd(key1, "Element1");
            redisDb.SetAdd(key1, "Element2");
            redisDb.SetAdd(key1, "Element3");
            redisDb.SetAdd(key1, "Element4");
            redisDb.SetAdd(key1, "Element5");

            RedisValue[] result1 = redisDb.SetMembers(key1);
            Assert.IsNotNull(result1);

            var key2 = "RedisSet2";

            redisDb.SetAdd(key2, "Element1");
            redisDb.SetAdd(key2, "Element2");
            redisDb.SetAdd(key2, "Element3");
            redisDb.SetAdd(key2, "Element4");
            redisDb.SetAdd(key2, "Element6");

            RedisValue[] result2 = redisDb.SetMembers(key2);
            Assert.IsNotNull(result2);

            var keys = new RedisKey[2];
            keys[0] = key2;
            keys[1] = key1;

            var result3 = redisDb.SetCombine(SetOperation.Difference, keys);
            Assert.IsNotNull(result3);

            redisDb.KeyDelete(key1);
            redisDb.KeyDelete(key2);
        }

        [TestMethod]
        public void Test_RedisHash()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisHash:User1";

            redisDb.HashSet(key, "UserName", "Raistlin");
            redisDb.HashSet(key, "CreditLimit", 10001);
            redisDb.HashSet(key, "Location", "Taiwan");

            var result = redisDb.HashGetAll(key);

            Assert.IsNotNull(result);

            Assert.AreEqual("Raistlin", result[0].Value.ToString());
            Assert.AreEqual(10001, result[1].Value);
            Assert.AreEqual("Taiwan", result[2].Value.ToString());

            redisDb.KeyDelete(key);
        }

        [TestMethod]
        public void Test_RedisSortedSet()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisSortedSet";

            redisDb.SortedSetAdd(key, "Element1", 10.0);
            redisDb.SortedSetAdd(key, "Element2", 20.0);
            redisDb.SortedSetAdd(key, "Element3", 30.0);
            redisDb.SortedSetAdd(key, "Element4", 40.0);
            redisDb.SortedSetAdd(key, "Element5", 50.0);

            var result = redisDb.SortedSetRank(key, "Element3", Order.Ascending);

            Assert.IsTrue(result == 2);

            redisDb.KeyDelete(key);
        }

        #endregion

        #region Advance

        #region Sort

        [TestMethod]
        public void Test_RedisSort()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisSort";

            redisDb.ListLeftPush(key, 2);
            redisDb.ListLeftPush(key, 4);
            redisDb.ListLeftPush(key, 1);
            redisDb.ListLeftPush(key, 3);
            redisDb.ListLeftPush(key, 5);

            var result = redisDb.Sort(key);

            Assert.IsNotNull(result);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);

            redisDb.KeyDelete(key);
        }

        [TestMethod]
        public void Test_RedisSortAlpha()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisSortAlpha";

            redisDb.ListLeftPush(key, "b");
            redisDb.ListLeftPush(key, "d");
            redisDb.ListLeftPush(key, "a");
            redisDb.ListLeftPush(key, "c");
            redisDb.ListLeftPush(key, "e");

            var result = redisDb.Sort(key, 0, -1, Order.Descending, SortType.Alphabetic);

            Assert.IsNotNull(result);
            Assert.IsTrue(result[0] == "e");
            Assert.IsTrue(result[1] == "d");
            Assert.IsTrue(result[2] == "c");
            Assert.IsTrue(result[3] == "b");
            Assert.IsTrue(result[4] == "a");

            redisDb.KeyDelete(key);
        }

        [TestMethod]
        public void Test_RedisSortBy()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisSortBy";

            redisDb.ListLeftPush(key, 2);
            redisDb.ListLeftPush(key, 4);
            redisDb.ListLeftPush(key, 1);
            redisDb.ListLeftPush(key, 3);
            redisDb.ListLeftPush(key, 5);

            redisDb.StringSet("w_1", "50");
            redisDb.StringSet("w_2", "100");
            redisDb.StringSet("w_3", "30");
            redisDb.StringSet("w_4", "20");
            redisDb.StringSet("w_5", "10");

            var by = "w_*";

            var result = redisDb.Sort(key, 0, -1, Order.Ascending, SortType.Numeric, by);

            Assert.IsTrue(result[0] == 5);
            Assert.IsTrue(result[1] == 4);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 1);
            Assert.IsTrue(result[4] == 2);

            var get = new RedisValue[1];
            get[0] = "w_*";
            var result2 = redisDb.Sort(key, 0, -1, Order.Ascending, SortType.Numeric, by, get);

            Assert.IsNotNull(result2);

            redisDb.KeyDelete(key);
            redisDb.KeyDelete("w_1");
            redisDb.KeyDelete("w_2");
            redisDb.KeyDelete("w_3");
            redisDb.KeyDelete("w_4");
            redisDb.KeyDelete("w_5");
        }

        [TestMethod]
        public void Test_RedisSortHash()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            string userId = "UserId";
            string key = "UserSetting";

            redisDb.ListLeftPush(userId, "0001");
            redisDb.HashSet(key + ":0001", "UserName", "User1");
            redisDb.HashSet(key + ":0001", "CreditLimit", 10001);

            redisDb.ListLeftPush(userId, "0002");
            redisDb.HashSet(key + ":0002", "UserName", "User2");
            redisDb.HashSet(key + ":0002", "CreditLimit", 10009);

            redisDb.ListLeftPush(userId, "0003");
            redisDb.HashSet(key + ":0003", "UserName", "User3");
            redisDb.HashSet(key + ":0003", "CreditLimit", 10003);

            redisDb.ListLeftPush(userId, "0004");
            redisDb.HashSet(key + ":0004", "UserName", "User4");
            redisDb.HashSet(key + ":0004", "CreditLimit", 10004);

            //SORT tag:ruby:posts BY post:*->time DESC GET post:*->title GET post:*->time GET #STORE sort.result

            var by = "UserSetting:*->CreditLimit";

            var get = new RedisValue[2];
            get[0] = "UserSetting:*->UserName";
            get[1] = "UserSetting:*->CreditLimit";

            var result = redisDb.Sort(userId, 0, -1, Order.Ascending, SortType.Numeric, by, get);

            Assert.IsNotNull(result);

            redisDb.KeyDelete(userId);
            redisDb.KeyDelete(key + ":0001");
            redisDb.KeyDelete(key + ":0002");
            redisDb.KeyDelete(key + ":0003");
            redisDb.KeyDelete(key + ":0004");
        }

        #endregion

        [TestMethod]
        public void Test_RedisExpire()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var expired = new TimeSpan(0, 0, 3);

            //Set Expire String
            redisDb.StringSet("RedisString", "ABC", expired);

            //String exist
            Assert.AreEqual(true, redisDb.KeyExists("RedisString"));

            Thread.Sleep(3001);

            //String not exist
            Assert.AreEqual(false, redisDb.KeyExists("RedisString"));
        }

        [TestMethod]
        public void Test_RedisMulti()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            redisDb.StringSet("TEST:ABC", 1234, null, When.Always, CommandFlags.FireAndForget);

            var key = "RedisTran:User1";

            //Set User
            redisDb.HashDelete(key, "UserName");
            redisDb.HashDelete(key, "CreditLimit");
            redisDb.HashDelete(key, "Location");

            redisDb.HashSet(key, "UserName", "Raistlin");
            redisDb.HashSet(key, "CreditLimit", 10001);
            redisDb.HashSet(key, "Location", "Taiwan");

            //Get User
            var result = redisDb.HashGetAll(key);

            Assert.IsNotNull(result);

            Assert.AreEqual("Raistlin", result[0].Value.ToString());
            Assert.AreEqual(10001, result[1].Value);
            Assert.AreEqual("Taiwan", result[2].Value.ToString());

            //Transaction in Condition
            var tran = redisDb.CreateTransaction();

            tran.AddCondition(Condition.HashEqual(key, "CreditLimit", 10001));
            tran.HashSetAsync(key, "CreditLimit", 10002);

            var exec = tran.ExecuteAsync();

            result = redisDb.HashGetAll(key);

            Assert.AreEqual(true, redisDb.Wait(exec));
            Assert.AreEqual("Raistlin", result[0].Value.ToString());
            Assert.AreEqual(10002, result[1].Value);
            Assert.AreEqual("Taiwan", result[2].Value.ToString());


            //Transaction not in Condition
            tran.AddCondition(Condition.HashEqual(key, "CreditLimit", 10001));
            tran.HashSetAsync(key, "CreditLimit", 10003);

            exec = tran.ExecuteAsync();

            result = redisDb.HashGetAll(key);

            Assert.AreEqual(false, redisDb.Wait(exec));
            Assert.AreEqual("Raistlin", result[0].Value.ToString());
            Assert.AreEqual(10002, result[1].Value);
            Assert.AreEqual("Taiwan", result[2].Value.ToString());

            redisDb.KeyDelete(key);
        }

        [TestMethod]
        public void TestPubSub()
        {
        }

        [TestMethod]
        public void Test_Redis_Pipeline()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis02");
            var tasks = new List<Task>();
            var idList = new List<int>();
            string key = "GMM:MARKETLINE:ID:{0}";
            for (int i = 1183665; i < 1214964; i++)
            {
                var id = i;
                var task = redisDb.KeyExistsAsync(string.Format(key, i)).ContinueWith(
                    t =>
                    {
                        if (t.Result)
                        {
                            idList.Add(id);
                        }
                    });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            Assert.AreNotEqual(0, idList.Count);
        }

        [TestMethod]
        public void Test_Redis_Batch()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            var batch = redisDb.CreateBatch();
            var tasks = new List<Task>();
            var idList = new List<int>();
            var sw = new Stopwatch();
            string key = "GMM:MARKETLINE:ID:{0}";
            for (int i = 1183665; i < 1214964; i++)
            {
                var id = i;
                var task = batch.KeyExistsAsync(string.Format(key, i)).ContinueWith(
                    t =>
                    {
                        if (t.Result)
                        {
                            idList.Add(id);
                        }
                    });
                tasks.Add(task);
            }
            batch.Execute();
            Task.WaitAll(tasks.ToArray());
            Assert.AreNotEqual(0, idList.Count);
        }
        #endregion

        #region Scripting

        [TestMethod]
        public void Test_Redis_LuaScript_Sort_MultiBy()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");

            var keyParm = new RedisKey[] { "UI:JsonEvents" };
            var argvParams = new RedisValue[]
                                 {
                                     "by", "UI:JsonEvent:ID:*->EventId", "by", "UI:JsonEvent:ID:*->CompetitionName",
                                     "ALPHA", "desc"
                                 };

            var luaScript = "return redis.call('sort'{0})";
            var luaParams = "";

            for (int i = 1; i <= keyParm.Length; i++)
            {
                luaParams += string.Format(" , KEYS[{0}]", i);
            }

            for (int i = 1; i <= argvParams.Length; i++)
            {
                luaParams += string.Format(" , ARGV[{0}]", i);
            }
            luaScript = string.Format(luaScript, luaParams);
            var result = (RedisValue[])redisDb.ScriptEvaluate(luaScript, keyParm, argvParams);

            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void Test_Redis_GetLocalization_MGet()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            var participantIds = new int[] { 1950, 1951, 1952, 1953, 1954, 1955, 1956, 1957 };
            var keys = new RedisKey[participantIds.Length];
            var keyPattern = "LOC:{0}:{1}:NAME:{2}";

            for (int i = 0; i <= participantIds.Length - 1; i++)
            {
                keys[i] = string.Format(keyPattern, "CHS", "PARTICIPANT", participantIds[i]);
            }
            var result = redisDb.StringGet(keys);

            for (int i = 0; i < participantIds.Length - 1; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.AreEqual("主场净赢1球", result[i].ToString());
                        break;
                    case 1:
                        Assert.AreEqual("主场淨贏2球", result[i].ToString());
                        break;
                    case 2:
                        Assert.AreEqual("主场淨贏3球", result[i].ToString());
                        break;
                    case 3:
                        Assert.AreEqual("主场淨贏4球或以上", result[i].ToString());
                        break;
                    case 4:
                        Assert.AreEqual("客场淨贏1球", result[i].ToString());
                        break;
                    case 5:
                        Assert.AreEqual("客场淨贏2球", result[i].ToString());
                        break;
                    case 6:
                        Assert.AreEqual("客场淨贏3球", result[i].ToString());
                        break;
                    case 7:
                        Assert.AreEqual("客场淨贏4球或以上", result[i].ToString());
                        break;
                }
            }
        }

        [TestMethod]
        public void Test_Redis_PreparedLuaScript()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            var script = @"local totalEventBetAmt = redis.call('GET', @KEY) + @STAKE
if (totalEventBetAmt > @EVENTBETLIMIT) then
	return 0
else
	return redis.call('INCRBY', @KEY, @STAKE)
end";

            var prepare = LuaScript.Prepare(script);
            var p =
                new
                {
                    KEY = (RedisKey)"TotalEventBetAmount",
                    @STAKE = (RedisValue)50.5,
                    @EVENTBETLIMIT = (RedisValue)1000.0
                };
            var result = prepare.Evaluate(redisDb, p);

            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void Test_Redis_LuaScript()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            string script = @"local totalEventBetAmt = redis.call('GET', KEYS[1])+ ARGV[1]
if (tostring(totalEventBetAmt) > ARGV[2]) then
	return ARGV[1]
else
	return redis.call('INCRBY', KEYS[1], ARGV[1])
end";

            var result = redisDb.ScriptEvaluate(
                script,
                new RedisKey[] { "TotalEventBetAmount" },
                new RedisValue[] { 50, 1000f });

            Assert.AreNotEqual(null, result);
        }

        #endregion

        [TestMethod]
        public void Test_Exam()
        {
            this.GenerateTestDataForTopTen();
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            var data = redisDb.HashGetAll("EXAM:WAGERS");
            redisDb.KeyDelete("EXAM:SORTEDWAGERS");

            foreach (var hashEntry in data)
            {
                var wager = JsonConvert.DeserializeObject<Wager>(hashEntry.Value);
                redisDb.SortedSetAdd("EXAM:SORTEDWAGERS:Jed.Lin", hashEntry.Value, wager.WinningAmount);
            }

            var topTenResult = redisDb.SortedSetRangeByScoreWithScores(
                "EXAM:SORTEDWAGERS:Jed.Lin",
                10000,
                order: Order.Descending,
                take: 10);

            foreach (var entry in topTenResult)
            {
                var wager = JsonConvert.DeserializeObject<Wager>(entry.Element);
                Console.WriteLine("Member {0} won {1} SGD!", wager.UserCode, wager.WinningAmount);
            }

            Assert.AreEqual(10, topTenResult.Length);
        }

        private void GenerateTestDataForTopTen()
        {
            var redisDb = RedisConnectionFactory.Instance.GetDatabase(GroupTypeEnum.Cache, "redis01");
            var rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 100; i++)
            {
                var stake = rnd.Next(100, 10000);
                var winningAmt = rnd.Next(-70, 30) * stake;

                var wager = new Wager
                {
                    WagerId = i + 1,
                    UserCode = "Member" + (i + 1).ToString().PadLeft(2, '0'),
                    Stake = stake,
                    WinningAmount = winningAmt <= 0 ? 0 : winningAmt,
                };
                redisDb.HashSet("EXAM:WAGERS", wager.WagerId, JsonConvert.SerializeObject(wager));
            }
        }

        internal class Wager
        {
            public long WagerId { get; set; }

            public string UserCode { get; set; }

            public long Stake { get; set; }

            public long WinningAmount { get; set; }
        }
    }
}
