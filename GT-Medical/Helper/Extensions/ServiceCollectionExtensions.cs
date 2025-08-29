using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GT_Medical.Helper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddByConvention(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
                assemblies = [Assembly.GetExecutingAssembly()];

            var allTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
                .ToArray();

            // Register services by lifetime markers
            RegisterWithLifetime(services, allTypes, typeof(Abstractions.ITransientService), ServiceLifetime.Transient);
            RegisterWithLifetime(services, allTypes, typeof(Abstractions.IScopedService), ServiceLifetime.Scoped);
            RegisterWithLifetime(services, allTypes, typeof(Abstractions.ISingletonService), ServiceLifetime.Singleton);

            return services;
        }

        private static void RegisterWithLifetime(
            IServiceCollection services,
            Type[] allTypes,
            Type markerInterface,
            ServiceLifetime lifetime)
        {
            var impls = allTypes
                .Where(t => markerInterface.IsAssignableFrom(t))
                .ToArray();

            foreach (var impl in impls)
            {
                // Register for all implemented interfaces except marker interfaces
                var serviceInterfaces = impl.GetInterfaces()
                    .Where(i => i != markerInterface &&
                                i != typeof(Abstractions.ITransientService) &&
                                i != typeof(Abstractions.IScopedService) &&
                                i != typeof(Abstractions.ISingletonService))
                    .ToArray();

                if (serviceInterfaces.Length == 0)
                {
                    // No interfaces? register self
                    services.Add(new ServiceDescriptor(impl, impl, lifetime));
                }
                else
                {
                    foreach (var itf in serviceInterfaces)
                    {
                        services.Add(new ServiceDescriptor(itf, impl, lifetime));
                    }
                }
            }
        }

        /// <summary>
        /// Registers all WinForms forms. By default: MainForm as Singleton, others as Transient.
        /// </summary>
        public static IServiceCollection AddFormsByConvention(
            this IServiceCollection services,
            Assembly assembly,
            Type? mainFormType = null)
        {
            var forms = assembly.GetTypes()
                .Where(t => typeof(Form).IsAssignableFrom(t) && t is { IsAbstract: false, IsClass: true })
                .ToArray();

            foreach (var formType in forms)
            {
                // Main form as singleton, others transient
                var isMain = mainFormType != null && formType == mainFormType;
                if (isMain)
                    services.AddSingleton(formType);
                else
                    services.AddTransient(formType);
            }

            return services;
        }
    }
}
