using Polly;
using ServiceDiscovery;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyDemo
{
    /// <summary>
    /// Polly是一种.Net 弹性和瞬间故障处理库，允许我们以非常顺畅和线程安全的方式来执行，如重试，断路，超时，故障恢复等策略；
    /// Nuget引入Polly库
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /*
            //使用格式
            Policy.Handle<Exception>(e => e.Message.Contains("Error")) //定义故障,可以对故障来检索，仅符合条件的会执行代码
                .Fallback(() => { Console.WriteLine("回退机制"); }) //用回退策略举例
                .Execute(() => { });//执行业务逻辑代码

            //各种策略：

            //1.回退降级策略，重试多少次之后还是不行就回出发回退降级，也就是垫底容错用的
            Policy.Handle<Exception>().Fallback(()=> {
                Console.WriteLine("");
            });

            //2.重试策略
            Policy.Handle<Exception>().Retry(2, (exception, i) =>
            {
                //记录错误信息
            });

            Policy.Handle<Exception>().RetryForever(); //永远重试

            //等待n时间后再重试，例如重试5次，委托里面定义每次等待多少秒
            Policy.Handle<Exception>().WaitAndRetry(5, i =>
            {
                return TimeSpan.FromSeconds(i);
            });
            //第1次重试等1秒，第2次等2秒，第3次等3秒
            Policy.Handle<Exception>().WaitAndRetry(new[] {
             TimeSpan.FromSeconds(1),
             TimeSpan.FromSeconds(2),
             TimeSpan.FromSeconds(3)
            });
            Policy.Handle<Exception>().WaitAndRetryForever(i =>
            {
                return TimeSpan.FromSeconds(i * 2);
            },
            (exce, t) =>
            {

            });

            //3.断路器策略
            Policy.Handle<Exception>().CircuitBreaker(2, TimeSpan.FromSeconds(10));//发送2次错误，则熔断10秒钟
            Policy.Handle<Exception>().AdvancedCircuitBreaker(  //高级熔断，在10秒内采样8次操作，如果故障数量超过50%，则熔断30秒
                 0.5,//故障数量大于50%
                 TimeSpan.FromSeconds(10), // 10秒采样时间
                 8, // 10秒内至少执行了8次操作
                 TimeSpan.FromSeconds(30));//熔断30秒

            //4.超时策略 超过3秒则触发
            Policy.Timeout(3).Execute(() => { Console.WriteLine("超时后执行"); }); //超时3秒后执行逻辑

            //5.舱壁隔离策略
            Policy.Bulkhead(12, context => //控制并发数量不超过12个
            {
                Console.WriteLine("来了第13个并发时，执行替代的操作");
            });//最多允许有有12个并发

            Policy.Bulkhead(12, 2, context =>
            {
                Console.WriteLine("来了第15个并发时，执行替代的操作");
            }); //控制并发数量不超过12个，当超过时，则取13,14进行排队，

            //6.缓存策略,还可以做缓存策略，可以支持Redis,Sql Service 等分布式缓存；需要再装一个包；

            

            //7.策略包装
            Policy.Wrap();//会把一些列的策略包装为一个包，然后从右往左执行；
              */

            //策略包装举例
            //var fallBack = Policy.Handle<Exception>().Fallback(() =>
            //{
            //    Console.WriteLine("Fallback");
            //});

            //var retry = Policy.Handle<Exception>().Retry(3, (exce, i) =>
            //  {
            //      Console.WriteLine($"重试次数:{i}");
            //  });

            //var policy = Policy.Wrap(fallBack, retry);

            ////调用打包的策略
            //policy.Execute(() =>
            //{
            //    //业务代码，对该业务代码执行打包的策略
            //    Console.WriteLine("Policy 测试开始");
            //    throw new Exception("error");//抛出错误出发策略
            //});

            //断路器，
            Policy.Handle<Exception>()
                .CircuitBreaker(
                2,//连续触发2次异常后进入到熔断状态，熔断时间为1分钟
                TimeSpan.FromMinutes(1));

            //高级断路器
            Policy.Handle<Exception>()
                .AdvancedCircuitBreaker(
                0.5,//故障数量大于50%
                TimeSpan.FromSeconds(10),//10秒采样时间
                8,//10秒内至少执行了8次操作，低于8次即使全部异常也不会触发熔断
                TimeSpan.FromSeconds(30)//熔断时间30秒
                );

            //下面测试下该微服务的例子
            //策略描述：
            /* 服务A,2个节点
               调用失败或者超时，就使用轮询方式进行重试调用，重试多次以后仍然失败，直接返回一个替代数据（服务降级）
               之后一段时间内，服务被熔断，熔断时间内，所有对该服务的调用都以替代数据返回。
               熔断时间过后，尝试再次调用，如果成功关掉熔断器；否则，再熔断一段时间
               包含的策略：超时策略、重试策略、熔断策略、回退策略


            测试方法：consul服务跑起来，MyServiceA服务跑起来3个实例，然后关掉其中一个，然后全部关掉，观察打印情况，
            用Ctril+f5跑起来该控制台，别调试因为throw 异常会报错；
            */


            #region 
            /*
            {
                var seviceProvider = new ConsulServiceProvider("http://127.0.0.1:8500");
                var serviceBuilder = seviceProvider.CreateServiceBuilder(builder =>
                {
                    builder.ServiceName = "MyServiceA";
                    builder.LoadBalancer = TypeLoadBalancer.RandomLoad;// new RoundRoinLoadBalancer();
                    builder.UriScheme = Uri.UriSchemeHttp;
                });

                HttpClient httpClient = new HttpClient();


                var policy = PolicyBuilder.CreatePolly();

                for (int i = 0; i < 100; i++)
                { 
                    Console.WriteLine($"----------------第{i}次请求-----------------------");
                     
                    policy.Execute(() =>
                    {
                        Task<Uri> url = serviceBuilder.BuildAsync("/Api/Health");
                        Uri apiUrl = url.Result;
                        Console.WriteLine($"{DateTime.Now}-正在调用{apiUrl}");
                        var content = httpClient.GetStringAsync(apiUrl).Result;
                        Console.WriteLine($"调用结果：{content}");
                    });

                    Task.Delay(1000).Wait();
                }


            }*/
            #endregion demo2 可以用来做一个动作后每隔一段时间后重试某一个动作 
            {
                //可以用来做一个动作后每隔一段时间后重试某一个动作
                var retryPolicy = Policy.HandleResult<TestResult>(aa => aa.Result == false)
                    .WaitAndRetry(3,
                    i => TimeSpan.FromSeconds(2),
                    (exception, span, retryCount, arg4) => //每次重试的时候都会执行的动作，一般用来做日志
                    {
                        Console.WriteLine($"{DateTime.Now} - 重试 {retryCount} 次");
                    });

                retryPolicy.Execute(SendMessage);
            }
        }

        public class TestResult
        {
            public bool Result { get; set; }
            public string Message { get; set; }
        }

        static TestResult SendMessage()
        {
            return new TestResult { Result = false, Message = "" };
        }
    }
}
