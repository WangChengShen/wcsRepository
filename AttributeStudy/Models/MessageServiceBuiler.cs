using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributeStudy.Models
{
    public class MessageServiceBuiler
    {
        private IServiceCollection _serviceCollection;
        public MessageServiceBuiler(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void SendSms()
        {
            _serviceCollection.AddTransient<IMessageService, SmsService>();
        }

        public void SendEmail()
        {
            _serviceCollection.AddTransient<IMessageService, EmailService>();
        }
    }

    /// <summary>
    /// IServiceCollection扩展方法
    /// </summary>
    public static class MessageServiceExtensions
    {
        public static void AddMessage(this IServiceCollection _serviceCollection )
        {
            var builder = new MessageServiceBuiler(_serviceCollection);
            builder.SendSms();
        }
        public static void AddMessage(this IServiceCollection _serviceCollection, Action<MessageServiceBuiler> configura)
        {
            var builder = new MessageServiceBuiler(_serviceCollection);
            configura(builder);
        }
    }

}
