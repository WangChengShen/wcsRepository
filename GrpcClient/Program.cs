using Grpc.Net.Client;
using GrpcServer;
using System;
using static GrpcServer.Greeter;

namespace GrpcClient
{
    /// <summary>
    /// grpc客户端是一个core的控制台程序项目，来调用grpc的服务，获取数据
    /// 1.引入几个包：
    ///     1.Google.Protobuf
    ///     2.Grpc.Net.Client
    ///     3.Grpc.Tools
    /// 2.把grpc服务的protos文件夹粘贴过来
    /// 3.项目文件配置（.csproj）文件配置）（双击项目打开）
    ///    <ItemGroup>
    ///        <Protobuf Include = "Protos\greet.proto" GrpcServices="Server" />
    ///      </ItemGroup>
    ///      
    /// 4.编译该项目，会根据配置生成请求服务的所有到的类
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //1、建立连接
            GrpcChannel grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");

            // 2、客户端创建
            GreeterClient greeterClient = new Greeter.GreeterClient(grpcChannel);

            // 3、开始调用   
            HelloReply helloReply = greeterClient.SayHello(new HelloRequest()
            {
                Name = "grpc客户端"
            });

            // 4、打印
            Console.WriteLine($"返回值打印：{helloReply.Message}");

            grpcChannel.Dispose();
        }
    }
}
