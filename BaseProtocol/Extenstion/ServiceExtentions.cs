using Microsoft.Extensions.DependencyInjection;
using System;

namespace BaseProtocol.Extenstion
{
    public static class ServiceExtentions
    {
        public static void AddDynamicService(this IServiceCollection services, Type type)
        {
            if (type.IsSubclassOf(typeof(SingletonService)))
            {
                services.AddSingleton(type);
            }
            else if (type.IsSubclassOf(typeof(ScopeServiceBase)))
            {
                services.AddScoped(type);
            }
            else if (type.IsSubclassOf(typeof(TransientServiceBase)))
            {
                services.AddTransient(type);
            }
        }
    }
}
