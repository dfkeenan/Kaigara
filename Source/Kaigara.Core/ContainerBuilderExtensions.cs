using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Kaigara.Collections.Generic;
using Kaigara.ViewModels;

namespace Kaigara
{
    public enum NamespaceRule
    {
        StartsWith,
        Equals
    }

    public static class ContainerBuilderExtensions
    {
        private static readonly string RegisteredModulesKey = $"{nameof(Kaigara)}.{nameof(ContainerBuilderExtensions)}.RegisteredModules";

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

        public static ContainerBuilder DependsOnModule<TModule>(this ContainerBuilder builder)
            where TModule : IModule, new()
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterModuleOnce<TModule>();

            return builder;
        }

        public static ContainerBuilder RegisterViewModels<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
            where TModule : IModule
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var moduleType = typeof(TModule);

            builder.RegisterAssemblyTypes(moduleType.Assembly)
                   .AssignableTo<ViewModel>()
                   .Where(GetNamespacePredicate(moduleType.Namespace!, namespaceRule))
                   .AsSelf()
                   .InstancePerDependency();

            return builder;
        }

        private static Func<Type, bool> GetNamespacePredicate(string @namespace, NamespaceRule namespaceRule)
        {
            return namespaceRule switch
            {
                NamespaceRule.StartsWith => (t) => t.Namespace!.StartsWith(@namespace),
                NamespaceRule.Equals => (t) => t.Namespace == @namespace,
                _ => throw new ArgumentOutOfRangeException(nameof(namespaceRule)),
            };
        }
    }
}
