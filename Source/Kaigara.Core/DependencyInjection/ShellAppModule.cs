using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Kaigara.Resources;
using Kaigara.ViewModels;

namespace Kaigara.DependencyInjection
{
    public enum NamespaceRule
    {
        StartsWith,
        Equals
    }

    public abstract class ShellAppModule : Module
    {
        public ShellAppModule()
        {
            var type = GetType();

            Namespace = type.Namespace!;
        }
        protected string Namespace { get; }

        protected override System.Reflection.Assembly ThisAssembly
        {
            get
            {
                Type type = GetType();
                Type baseType = type.BaseType!;
                if (baseType != typeof(ShellAppModule))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionResources.ThisAssemblyUnavailable, type, baseType));
                }

                return type.Assembly;
            }
        }

        protected void RegisterViewModels(ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        {

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .AssignableTo<ViewModel>()
                   .Where(GetNamespacePredicate(namespaceRule))
                   .AsSelf()
                   .InstancePerDependency();
        }

        protected Func<Type, bool> GetNamespacePredicate(NamespaceRule namespaceRule)
        {
            return namespaceRule switch
            {
                NamespaceRule.StartsWith => (t) => t.Namespace!.StartsWith(Namespace),
                NamespaceRule.Equals => (t) => t.Namespace == Namespace,
                _ => throw new ArgumentOutOfRangeException(nameof(namespaceRule)),
            };
        }

        protected void DependsOnModule<TModule>(ContainerBuilder builder)
            where TModule : IModule, new()
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterModuleOnce<TModule>();
        }
    }
}
