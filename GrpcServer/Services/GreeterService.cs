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
    /// GRPC����vs�ṩ��grpc��Ŀ��ģ�壬�����Ŀ=>ѡ��grpc��ģ�弴��
    /// �������У���Ŀ�ļ�����ִ�У�dotnet run 
    /// ��debug�ļ����£� dotnet GrpcServer.dll --urls='http://*:5002'
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ���񷽷���ʵ��
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"��ȡ���������{request.Name}");
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        /// <summary>
        /// ���񷽷���ʵ��
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>
        public override Task<GetUserListResp> GetUserList(GetUserListReq request, ServerCallContext context)
        {
            Console.WriteLine($"��ȡ���������{request.Name}");

            List<UserInfo> userList = new List<UserInfo> {
                new UserInfo{
                 Name="����",
                 Handset="15110011001",
                 Address="����"
                },
                new UserInfo{
                 Name="����",
                 Handset="15110011002",
                 Address="�Ϻ�"
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
