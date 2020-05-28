using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisDemo
{

    public class ZSetBase
    {
        public double Score { get; set; }
    }

    public class GiftCount : ZSetBase
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>
    /// 需引用ServiceStack.Redis.dll
    /// redis客户端StackExchange的使用 Redis读写帮助类
    /// Redis可操作的数据类型：String ，Hash ，List ，Set ，和 Sorted Set
    /// </summary>
    public class RedisHelper
    {
        public static ConnectionMultiplexer connectionMultiplexer { get; set; }
        //private static IDatabase db { get; set; }

        public static IDatabase db { get; set; } //应该私有化，但是为了在外部操作，在此公开化

        private static object obj = new object();

        /// <summary>
        /// ConnectionMultiplexer对象是StackExchange.Redis最中枢的对象。
        /// 这个类的实例需要被整个应用程序域共享和重用的，你不要在每个操作中不停的创建该对象的实例,所以使用单例来创建和存放这个对象是必须的
        /// </summary>
        /// <returns></returns>
        private static ConnectionMultiplexer InitConnection()
        {

            if (connectionMultiplexer == null)
            {
                lock (obj)
                {
                    if (connectionMultiplexer == null)
                    {
                        //"127.0.0.1:6379,127.0.0.1:6380,ssl=true,password=123,,allowAdmin=true"
                        string connection = "127.0.0.1:6379,allowAdmin=true,password=123456"; //ConfigurationManager.AppSettings["RedisConnStr"].ToString();

                        /*ConfigurationOptions 包含大量的配置选项，一些常用的配置如下：
                            abortConnect ： 当为true时，当没有可用的服务器时则不会创建一个连接
                            allowAdmin ： 当为true时 ，可以使用一些被认为危险的命令
                            channelPrefix：所有pub/sub渠道的前缀
                            connectRetry ：重试连接的次数
                            connectTimeout：超时时间
                            configChannel： Broadcast channel name for communicating configuration changes
                            defaultDatabase ： 默认0到-1
                            keepAlive ： 保存x秒的活动连接
                            name:ClientName
                            password:password
                            proxy:代理 比如 twemproxy
                            resolveDns : 指定dns解析
                            serviceName ： Not currently implemented (intended for use with sentinel)
                            ssl={bool} ： 使用sll加密
                            sslHost={string} ： 强制服务器使用特定的ssl标识
                            syncTimeout={int} ： 异步超时时间
                            tiebreaker={string}：Key to use for selecting a server in an ambiguous master scenario
                            version={string} ： Redis version level (useful when the server does not make this available)
                            writeBuffer={int} ： 输出缓存区的大小*/

                        //1.初始化方法一
                        //ConfigurationOptions option = new ConfigurationOptions()
                        //{
                        //    AllowAdmin = true,
                        //    ConnectRetry = 10

                        //};
                        // connectionMultiplexer = ConnectionMultiplexer.Connect(option);

                        //2:初始化方法二：
                        connectionMultiplexer = ConnectionMultiplexer.Connect(connection);

                        db = connectionMultiplexer.GetDatabase();

                        /*其他：
                         * StackExchange.Redis 使用 - 事件  ConnectionMultiplexer 可以注册如下事件：
                           ConfigurationChanged - 配置更改时
                           ConfigurationChangedBroadcast - 通过发布订阅更新配置时
                           ConnectionFailed - 连接失败 ， 如果重新连接成功你将不会收到这个通知
                           ConnectionRestored - 重新建立连接之前的错误
                           ErrorMessage - 发生错误
                           HashSlotMoved - 更改集群
                           InternalError - redis类库错误
                         */
                    }
                }
            }
            return connectionMultiplexer;
        }

        static RedisHelper()
        {
            RedisHelper.InitConnection();
        }

        #region string类型操作
        /// <summary>
        /// set or update the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetStringValue(string key, string value)
        {
            return db.StringSet(key, value);
        }
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static bool SetStringKey(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return db.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetStringKey<T>(string key) where T : class
        {
            var result = db.StringGet(key);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
        /// <summary>
        /// get the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringValue(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// Delete the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteStringKey(string key)
        {
            return db.KeyDelete(key);
        }

        public static bool StringSetList<T>(string key, List<T> list)
        {
            return db.StringSet(key, JsonConvert.SerializeObject(list));
        }

        public static List<T> GetStringList<T>(string key)
        {
            string valString = db.StringGet(key);
            return JsonConvert.DeserializeObject<List<T>>(valString);
        }

        /// <summary>
        /// 取值并且把值减去1操作，二合一操作，可用来做秒杀
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long StringDecrement(string key)
        {
            return db.StringDecrement(key);
        }

        #endregion

        #region 哈希类型操作
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetHashValue(string key, string hashkey, string value)
        {
            return db.HashSet(key, hashkey, value);
        }
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="t">defined class</param>
        /// <returns></returns>
        public static bool SetHashValue<T>(String key, string hashkey, T t) where T : class
        {
            var json = JsonConvert.SerializeObject(t);
            return db.HashSet(key, hashkey, json);
        }
        /// <summary>
        /// 保存一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="list">数据集合</param>
        /// <param name="getModelId"></param>
        public static void HashSet<T>(string key, List<T> list, Func<T, string> getModelId)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonConvert.SerializeObject(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            db.HashSet(key, listHashEntry.ToArray());
        }
        /// <summary>
        /// 获取hashkey所有的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashGetAll<T>(string key) where T : class
        {
            List<T> result = new List<T>();
            HashEntry[] arr = db.HashGetAll(key);
            foreach (var item in arr)
            {
                if (!item.Value.IsNullOrEmpty)
                {
                    //if (JsonHelper.DeserializeJsonToObject<T>(item.Value, out T t))
                    //{
                    result.Add(JsonConvert.DeserializeObject<T>(item.Value));
                    //}

                }
            }
            return result;
            //result =JsonHelper.DeserializeJsonToList<T>(arr.ToString());                        
            //return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public static RedisValue GetHashValue(string key, string hashkey)
        {
            RedisValue result = db.HashGet(key, hashkey);
            return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public static T GetHashValue<T>(string key, string hashkey) where T : class
        {
            RedisValue result = db.HashGet(key, hashkey);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(result);
            //if (JsonHelper.DeserializeJsonToObject(result, out T t))
            //{
            //    return t;
            //}
            //return null;
        }
        /// <summary>
        /// delete the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public static bool DeleteHashValue(string key, string hashkey)
        {
            return db.HashDelete(key, hashkey);
        }
        #endregion

        #region List的操作
        /*Redis 的List类型和 .NET领域还有所不同，实际上，它是一个双向队列，可以左右插入值。所以如果是批量数据插入 那么必须一个个插入， 代码比较简单如下:*/
        public static bool SetListValue<T>(string key, List<T> valList)
        {
            foreach (var itemVal in valList)
            {
                //db.ListLeftPush 添加到左边
                db.ListRightPush(key, JsonConvert.SerializeObject(itemVal)); //要一个个的插入
            }
            return true;
        }

        public static List<T> ListGet<T>(string key)
        {
            RedisValue[] rvList = db.ListRange(key);

            List<T> valList = new List<T>();
            foreach (var item in rvList)
            {
                valList.Add(JsonConvert.DeserializeObject<T>(item));
            }
            return valList;
        }

        #endregion

        #region set的操作

        /// <summary>
        /// sortedSet 是可以
        /// </summary>
        public static bool SetSetValue<T>(string key, List<T> valList)
        {
            foreach (var itemVal in valList)
            {
                db.SetAdd(key, JsonConvert.SerializeObject(itemVal));
            }
            return true;
        }

        public static List<T> SetGet<T>(string key)
        {
            RedisValue[] rvList = db.SetMembers(key);

            List<T> valList = new List<T>();
            foreach (var item in rvList)
            {
                valList.Add(JsonConvert.DeserializeObject<T>(item));
            }
            return valList;
        }

        #endregion

        #region zset的操作
        public static bool ZSetSetValue(string key, string str, double score)
        {
            db.SortedSetAdd(key, str, score);
            return true;
        }

        public static Dictionary<string, double> ZSetGetValAndScoreList(string key)
        {
            RedisValue[] rvList = db.SortedSetRangeByRank(key, 0, 100, order: Order.Descending);

            Dictionary<string, double> valList = new Dictionary<string, double>();

            foreach (var item in rvList)
            {
                valList.Add(item, db.SortedSetScore(key, item) ?? 0);
            }
            return valList;
        }

        public static bool ZSetSetValue<T>(string key, List<T> valList) where T : ZSetBase
        {
            foreach (var itemVal in valList)
            {
                db.SortedSetAdd(key, JsonConvert.SerializeObject(itemVal), itemVal.Score);
            }
            return true;
        }

        public static List<T> ZSetGet<T>(string key) where T : ZSetBase
        {
            // RedisValue[] rvList = db.SortedSetRangeByScore(key, 0, 100, order: Order.Descending);
            RedisValue[] rvList = db.SortedSetRangeByRank(key, 0, 100, order: Order.Descending);
            List<T> valList = new List<T>();
            foreach (var item in rvList)
            {
                T t = JsonConvert.DeserializeObject<T>(item);
                t.Score = db.SortedSetScore(key, item) ?? 0;
                valList.Add(t);
            }
            return valList;
        }

        /// <summary>
        /// 修改zset某个元素的权重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="addScore"></param>
        /// <returns></returns>
        public static double ZSetSetValueScore<T>(string key, T t, double addScore) where T : ZSetBase
        {
            return db.SortedSetIncrement(key, JsonConvert.SerializeObject(t), addScore);
        }

        public static double ZSetSetStringValueScore(string key, string str, double addScore)
        {
            return db.SortedSetIncrement(key, str, addScore);
        }
        #endregion



    }
}
