﻿using Learning.Domain;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
namespace Learning.AspNetCore
{
    public static class ServiceCollectionExtension
    {
        public static List<Assembly> assemblies = new List<Assembly>();
        public static IServiceCollection RunModuleInitializers(this IServiceCollection services)
        {
            LoadAllAssemblies();
            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
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

        /// <summary>
        /// 批量注册服务
        /// TODO: 还未实现注入普通类，不实现接口
        /// </summary>
        /// <param name="services"></param>
        public static void AutomaticRegisterService(this IServiceCollection services)
        {
            LoadAllAssemblies();
            //// 获取入口程序的目录路径
            //string entryAssemblyLocation = Assembly.GetEntryAssembly()!.Location;
            //string entryDirectory = Path.GetDirectoryName(entryAssemblyLocation)!;

            //// 获取目录下的所有程序集文件
            //string[] assemblyFiles = Directory.GetFiles(entryDirectory, "*.dll");
            //List<Assembly> list = new List<Assembly>();
            //foreach (string assemblyFile in assemblyFiles)
            //{
            //    list.Add(Assembly.LoadFrom(assemblyFile));
            //}
            //string path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            //var referencedAssemblies = Directory.GetFiles(path, "IdentityService.*.dll").Select(Assembly.LoadFrom).ToArray();

            var allTypes = assemblies.SelectMany(a => a.GetTypes());

            #region interfaces
            // 实现接口方式注入服务
            var interFaceTypes = allTypes.Where(t => typeof(IDependency).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.IsGenericType);
            //var interFaceTypes = allTypes.Where(t => t.IsAssignableFrom(typeof(IScopedDependency)) && t.IsClass && !t.IsAbstract && !t.IsGenericType).ToList();// && t.IsClass && !t.IsAbstract && !t.IsGenericType);
            foreach (var type in interFaceTypes)
            {
                Type[] interfaceTypes = type.GetInterfaces();

                var types = interfaceTypes.Where(t => t != typeof(IDependency)
                    && t != typeof(ISingletonDependency)
                    && t != typeof(IScopedDependency)
                    && t != typeof(ITransientDependency)).ToArray();

                if (interfaceTypes.Contains(typeof(IScopedDependency)))
                {
                    AddScoped(services, type, types);
                }
                else if (interfaceTypes.Contains(typeof(ISingletonDependency)))
                {
                    AddSingleton(services, type, types);
                }
                else
                {
                    AddTransient(services, type, types);
                }
            }

            #endregion
        }

        private static void LoadAllAssemblies()
        {
            if (assemblies.Any())
            {
                return;
            }
            // 获取入口程序的目录路径
            string entryAssemblyLocation = Assembly.GetEntryAssembly()!.Location;
            string entryDirectory = Path.GetDirectoryName(entryAssemblyLocation)!;

            // 获取目录下的所有程序集文件
            string[] assemblyFiles = Directory.GetFiles(entryDirectory, "*.dll");
            foreach (string assemblyFile in assemblyFiles)
            {
                string fileName = Path.GetFileName(assemblyFile);
                if (fileName == "System.Private.CoreLib.dll")
                {
                    continue;
                }
                assemblies.Add(Assembly.LoadFrom(assemblyFile));
            }
        }

        private static void AddScoped(IServiceCollection services, Type classType, Type[]? iTypes)
        {
            if (iTypes == null || !iTypes.Any())
            {
                services.AddScoped(classType);
                return;
            }
            foreach (var type in iTypes)
            {
                services.AddScoped(type, classType);
            }
        }

        private static void AddSingleton(IServiceCollection services, Type classType, Type[]? iTypes)
        {
            if (iTypes == null || !iTypes.Any())
            {
                services.AddSingleton(classType);
                return;
            }
            foreach (var type in iTypes)
            {
                services.AddSingleton(type, classType);
            }

        }

        private static void AddTransient(IServiceCollection services, Type classType, Type[]? iTypes)
        {
            if (iTypes == null || !iTypes.Any())
            {
                services.AddTransient(classType);
                return;
            }
            foreach (var type in iTypes)
            {
                services.AddTransient(type, classType);
            }

        }
    }
}
