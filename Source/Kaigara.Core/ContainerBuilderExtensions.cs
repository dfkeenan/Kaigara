using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Collections.Generic;
using Kaigara.Commands;
using Kaigara.Menus;
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

            return builder.RegisterViewModels(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
        }

        public static ContainerBuilder RegisterViewModels(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            var namespacePredicate = GetNamespacePredicate(@namespace!, namespaceRule);

            builder.RegisterAssemblyTypes(assembly)
                               .AssignableTo<ViewModel>()
                               .Where(t => namespacePredicate(t))
                               .AsSelf()
                               .InstancePerDependency();
            return builder;
        }

        public static ContainerBuilder RegisterMenus<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
            where TModule : IModule
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var moduleType = typeof(TModule);

            return builder.RegisterMenus(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
        }

        public static ContainerBuilder RegisterMenus(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            var namespacePredicate = GetNamespacePredicate(@namespace!, namespaceRule);

            builder.RegisterAssemblyTypes(assembly)
                               .AssignableTo<MenuViewModel>()
                               .Where(t => namespacePredicate(t))
                               .OnActivating(e =>
                               {
                                   if(e.Instance is MenuViewModel m)
                                   {
                                       e.Context.Resolve<IMenuManager>().Register(m.Definition);
                                   }
                               })
                               .AsSelf()
                               .InstancePerDependency();
            return builder;
        }

        public static ContainerBuilder RegisterCommands<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
           where TModule : IModule
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var moduleType = typeof(TModule);

            return builder.RegisterCommands(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
        }

        public static ContainerBuilder RegisterCommands(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            var namespacePredicate = GetNamespacePredicate(@namespace!, namespaceRule);

            builder.RegisterAssemblyTypes(assembly)
                               .AssignableTo<RegisteredCommandBase>()
                               .Where(t => namespacePredicate(t))
                               .AsSelf()
                               .As<RegisteredCommandBase>()
                               .SingleInstance();
            return builder;
        }

        public static ContainerBuilder RegisterDocuments<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
            where TModule : IModule
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var moduleType = typeof(TModule);

            return builder.RegisterDocuments(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
        }

        public static ContainerBuilder RegisterDocuments(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            var namespacePredicate = GetNamespacePredicate(@namespace!, namespaceRule);

            //Documents
            builder.RegisterAssemblyTypes(assembly)
                               .AssignableTo<IDocument>()
                               .Where(t => namespacePredicate(t))
                               .AsSelf()
                               .InstancePerDependency();

            return builder;
        }

        public static ContainerBuilder RegisterTools<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
            where TModule : IModule
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var moduleType = typeof(TModule);

            return builder.RegisterTools(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
        }
        public static ContainerBuilder RegisterTools(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            var namespacePredicate = GetNamespacePredicate(@namespace!, namespaceRule);

            builder.RegisterAssemblyTypes(assembly)
                               .AssignableTo<ITool>()
                               .Where(t => namespacePredicate(t))
                               .AsSelf()
                               .AsImplementedInterfaces()
                               .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterAllModels<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
            where TModule : IModule
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var moduleType = typeof(TModule);

            return builder.RegisterAllModels(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
        }

        public static ContainerBuilder RegisterAllModels(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            builder.RegisterViewModels(assembly, @namespace, namespaceRule);
            builder.RegisterDocuments(assembly, @namespace, namespaceRule);
            builder.RegisterTools(assembly, @namespace, namespaceRule);
            builder.RegisterMenus(assembly, @namespace, namespaceRule);
            builder.RegisterCommands(assembly, @namespace, namespaceRule);

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
