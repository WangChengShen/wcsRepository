using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisDemo.Demo
{
    public class RedisMq
    {
		/// <summary>
		/// 发布消息
		/// </summary>
		public static void PublishMsg()
		{
			try
			{
				//创建一个公众号--创建一个主题
				Console.WriteLine("发布服务");
				IRedisClientsManager redisClientManager = new PooledRedisClientManager("127.0.0.1:6379");
				string topicname = "Send_Log";//发布订阅消息的标识
				RedisPubSubServer pubSubServer = new RedisPubSubServer(redisClientManager, topicname)
				{
					//收到消息事件，发布消息服务可以不注册此事件
					OnMessage = (channel, msg) =>
					{
						Console.WriteLine($"从频道：{channel}上接受到消息：{msg},时间：{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}");
						Console.WriteLine("___________________________________________________________________");
					},
					OnStart = () =>//服务启动出发事件
					{
						Console.WriteLine("发布服务已启动");
						Console.WriteLine("___________________________________________________________________");
					},
					OnStop = () => { Console.WriteLine("发布服务停止"); },
					OnUnSubscribe = channel => { Console.WriteLine(channel); },
					OnError = e => { Console.WriteLine(e.Message); },
					OnFailover = s => { Console.WriteLine(s); },
				};
				//接收消息
				pubSubServer.Start();
				while (true)
				{
					Console.WriteLine("请输入记录的日志");
					string message = Console.ReadLine();
					redisClientManager.GetClient().PublishMessage(topicname, message);
				}
			}
			catch (Exception ex)
			{ 
				Console.WriteLine(ex.Message);
			} 
		}

		/// <summary>
		/// 消费者订阅消息
		/// </summary>
		public static void SubscriptionMsg()
        {
			try
			{
				using (RedisClient consumer = new RedisClient("127.0.0.1", 6379))
				{
					Console.WriteLine($"创建订阅异常信息数据库记录");
					var subscription = consumer.CreateSubscription();
					//接受到消息时
					subscription.OnMessage = (channel, msg) =>
					{
						if (msg != "CTRL:PULSE")
						{
							Console.WriteLine($"从频道：{channel}上接受到消息：{msg},时间：{DateTime.Now.ToString("yyyyMMdd HH:mm:sss")}");
							//Logger.WriteLogByDB(msg);
							//Console.WriteLine("_________________________________记录成功__________________________________");
						}
					};
					//订阅频道时
					subscription.OnSubscribe = (channel) =>
					{
						Console.WriteLine("订阅客户端：开始订阅" + channel);
					};
					//取消订阅频道时
					subscription.OnUnSubscribe = (a) => { Console.WriteLine("订阅客户端：取消订阅"); };

					//订阅频道
					string topicname = "Send_Log";
					subscription.SubscribeToChannels(topicname);
				}
			}
			catch (Exception ex)
			{ 
				Console.WriteLine(ex.Message);
			}
		}


		
	}
}
