using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Kaigara.Collections.Generic;

namespace Kaigara.DependencyInjection
{
    public static class ContainerBuilderExtensions
    {
        private static readonly string RegisteredModulesKey = $"{nameof(Kaigara)}.{nameof(DependencyInjection)}.{nameof(ContainerBuilderExtensions)}.RegisteredModules";

        public static ContainerBuilder RegisterModuleOnce<TModule>(this ContainerBuilder builder)
            where TModule : IModule, new()
        {
            return builder.RegisterModuleOnce(new TModule());
        }

        public static ContainerBuilder RegisterModuleOnce<TModule>(this ContainerBuilder builder, TModule module)
            where TModule : IModule
        {
            var registeredModules = GetRegisteredModules(builder);

            var moduleTypeName = typeof(TModule).AssemblyQualifiedName!;


            if (registeredModules.Add(moduleTypeName))
            {
                builder.RegisterModule(module);
            }
            return builder;
        }

        private static HashSet<string> GetRegisteredModules(ContainerBuilder builder)
        {
            return (HashSet<string>)builder.Properties.GetOrAdd(RegisteredModulesKey, _ => new HashSet<string>())!;
        }
    }
}
