using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrpcServer
{

    /// <summary>
    /// GRPC服务，vs提供了grpc项目的模板，添加项目=>选择grpc的模板即可
    /// 服务运行：项目文件夹下执行：dotnet run 
    /// 或debug文件夹下： dotnet GrpcServer.dll --urls='http://*:5002'
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 服务方法的实现
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"获取请求参数：{request.Name}");
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        /// <summary>
        /// 服务方法的实现
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>
        public override Task<GetUserListResp> GetUserList(GetUserListReq request, ServerCallContext context)
        {
            Console.WriteLine($"获取请求参数：{request.Name}");

            List<UserInfo> userList = new List<UserInfo> {
                new UserInfo{
                 Name="张三",
                 Handset="15110011001",
                 Address="北京"
                },
                new UserInfo{
                 Name="李四",
                 Handset="15110011002",
                 Address="上海"
                }
            };

            return Task.FromResult(new GetUserListResp
            {
                UserListInfo = JsonConvert.SerializeObject(userList)
            });
        }


        public class UserInfo
        {
            public string Name { get; set; }
            public string Handset { get; set; }
            public string Address { get; set; }
        }
    }
}
