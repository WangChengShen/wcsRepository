using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace PollyDemo
{
   public static class PolicyBuilder
    { 
        public static ISyncPolicy CreatePolly()
        {
            var timeoutPolicy = Policy.Timeout(2, (context, span, arg3) =>
            {
                Console.WriteLine("执行超时");
            });

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(2,
                    i => TimeSpan.FromSeconds(2),
                    (exception, span, retryCount, arg4) =>
                    {
                        Console.WriteLine($"{DateTime.Now} - 重试 {retryCount} 次");
                    });

            var cirecuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreaker(
                    2,
                    TimeSpan.FromSeconds(5),
                    (exception, span) =>
                    {
                        Console.WriteLine($"{DateTime.Now} - 断路器：开启状态（熔断时触发）");
                    },
                    () =>
                    {
                        Console.WriteLine($"{DateTime.Now} - 断路器：关闭状态（熔断恢复时触发）");
                    },
                    () =>
                    {
                        Console.WriteLine($"{DateTime.Now} - 断路器：半开启状态（熔断时间到了之后触发）");
                    });

            var fallbackPolicy = Policy.Handle<Exception>()
                .Fallback(() =>
                {
                    Console.WriteLine("这是一个替代数据");
                }, exception =>
                {
                    Console.WriteLine("Fallback被触发");
                });

            return Policy.Wrap(fallbackPolicy, cirecuitBreakerPolicy, retryPolicy, timeoutPolicy);
        }
    }
}
