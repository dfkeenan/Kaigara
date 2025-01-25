using System.Reflection;
using Autofac;
using Autofac.Core;
using Dock.Model.Controls;
using Kaigara.Collections.Generic;
using Kaigara.Commands;
using Kaigara.Configuration;
using Kaigara.Configuration.UI.ViewModels;
using Kaigara.Dialogs;
using Kaigara.Dialogs.Commands;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Menus;
using Kaigara.Reflection;
using Kaigara.Toolbars;
using Kaigara.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Kaigara;

public enum NamespaceRule
{
    StartsWith,
    Equals
}

public static class ContainerBuilderExtensions
{
    private static readonly string RegisteredModulesKey = $"{nameof(Kaigara)}.{nameof(ContainerBuilderExtensions)}.RegisteredModules";

    private static readonly MethodInfo configureMethod
        = typeof(ContainerBuilderExtensions)
          .GetMethod(nameof(Configure), 1, new[] { typeof(ContainerBuilder), typeof(string) })!;

    public static ContainerBuilder Configure(this ContainerBuilder builder, Type type, string? name = null)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        configureMethod.MakeGenericMethod(type).Invoke(null, new object?[] { builder, name });

        return builder;
    }

    public static ContainerBuilder Configure<TOptions>(this ContainerBuilder builder, string? name = null)
        where TOptions : class
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        name ??= typeof(TOptions).FullName;

        builder.Register((IConfiguration config) => new ConfigurationChangeTokenSource<TOptions>(Options.DefaultName, config.GetSection(name!)))
                .As<IOptionsChangeTokenSource<TOptions>>()
                .SingleInstance();

        builder.Register((IConfiguration config) => new NamedConfigureFromConfigurationOptions<TOptions>(Options.DefaultName, config.GetSection(name!)))
               .As<IConfigureOptions<TOptions>>()
               .SingleInstance();

        if (typeof(TOptions).IsAssignableTo<IOptionsModel>())
        {
            builder.Register((IOptionsSnapshot<TOptions> options) => options.Value)
            .As<IOptionsModel>()
            .InstancePerDependency();
        }

        return builder;
    }

    public static ContainerBuilder Configure<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
        where TModule : IModule
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var moduleType = typeof(TModule);

        return builder.Configure(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
    }

    public static ContainerBuilder Configure(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
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


        var configTypes = from type in assembly.GetTypes()
                          where type.IsAssignableTo(typeof(IOptionsModel))
                             && namespacePredicate(type)
                          select type;

        foreach (var type in configTypes)
        {
            builder.Configure(type);
        }

        return builder;
    }

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
                           .AsImplementedInterfaces()
                           .InstancePerDependency();

        builder.RegisterAssemblyTypes(assembly)
                           .AssignableTo<IDialogViewModel>()
                           .Where(t => namespacePredicate(t) && t.HasAttribute<ShowDialogCommandDefinitionAttribute>())
                           .As<IDialogViewModel>()
                           .WithMetadata(DialogsModule.MetadataName, t => t.GetCustomAttributes())
                           .InstancePerDependency();

        //TODO : Should this be moved somewhere else
        builder.RegisterAssemblyTypes(assembly)
                           .AssignableTo<OptionsPageViewModel>()
                           .Where(t => namespacePredicate(t))
                           .As<OptionsPageViewModel>()
                           .AsSelf()
                           .InstancePerDependency()
                           .WithMetadataFrom<OptionsPageAttribute>();
        return builder;
    }

    public static ContainerBuilder RegisterMenu(this ContainerBuilder builder, MenuDefinition definition)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (definition is null)
        {
            throw new ArgumentNullException(nameof(definition));
        }

        builder.Register(c => new MenuViewModel(definition))
            .OnActivating(e =>
            {
                if (e.Instance is MenuViewModel m)
                {
                    e.Context.Resolve<IMenuManager>().Register(m.Definition);
                }
            })
            .AsSelf()
            .SingleInstance()
            .Keyed<MenuViewModel>(definition.Name);

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
                               if (e.Instance is MenuViewModel m)
                               {
                                   e.Context.Resolve<IMenuManager>().Register(m.Definition);
                               }
                           })
                           .AsSelf()
                           .SingleInstance();
        return builder;
    }

    public static ContainerBuilder RegisterToolbarTray(this ContainerBuilder builder, ToolbarTrayDefinition definition)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (definition is null)
        {
            throw new ArgumentNullException(nameof(definition));
        }

        builder.Register(c => new ToolbarTrayViewModel(definition))
            .OnActivating(e =>
            {
                if (e.Instance is ToolbarTrayViewModel m)
                {
                    e.Context.Resolve<IToolbarManager>().Register(m.Definition);
                }
            })
            .AsSelf()
            .SingleInstance()
            .Keyed<ToolbarTrayViewModel>(definition.Name);

        return builder;
    }

    public static ContainerBuilder RegisterToolbars<TModule>(this ContainerBuilder builder, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
       where TModule : IModule
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var moduleType = typeof(TModule);

        return builder.RegisterToolbars(moduleType.Assembly, moduleType.Namespace!, namespaceRule);
    }

    public static ContainerBuilder RegisterToolbars(this ContainerBuilder builder, Assembly assembly, string @namespace, NamespaceRule namespaceRule = NamespaceRule.StartsWith)
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
                           .AssignableTo<ToolbarTrayViewModel>()
                           .Where(t => namespacePredicate(t))
                           .OnActivating(e =>
                           {
                               if (e.Instance is ToolbarTrayViewModel m)
                               {
                                   e.Context.Resolve<IToolbarManager>().Register(m.Definition);
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
                           .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                           .As<RegisteredCommandBase>()
                           .SingleInstance()
                           .AutoActivate()
                           .OnActivated((e) =>
                           {
                               var command = (RegisteredCommandBase)e.Instance;
                               var manager = e.Context.Resolve<ICommandManager>();
                               manager.RegisterCommand(command);
                           });
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
        builder.RegisterToolbars(assembly, @namespace, namespaceRule);
        builder.RegisterCommands(assembly, @namespace, namespaceRule);
        builder.Configure(assembly, @namespace, namespaceRule);
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
