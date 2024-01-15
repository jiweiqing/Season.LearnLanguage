using IdentityService.Domain;
using System.ComponentModel.Design;
using System.Reflection;

namespace IdentityService.Host
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RunModuleInitializers(this IServiceCollection services)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                var moduleTypes = types.Where(t => !t.IsAbstract && typeof(IModuleInitializer).IsAssignableFrom(t));
                if (moduleTypes != null)
                {
                    foreach (var type in moduleTypes)
                    {
                        var initializer = (IModuleInitializer?)Activator.CreateInstance(type);
                        if (initializer != null)
                        {
                            initializer.Initialize(services);
                        }
                    }
                }
            }
            return services;
        }
    }
}
