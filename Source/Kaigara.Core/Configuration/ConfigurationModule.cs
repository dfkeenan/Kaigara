using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kaigara.Configuration;
public class ConfigurationModule : Module
{
    public string? UserAppsettingFilePath { get; set; }

    public bool IncludeDefaultUI { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.Register((IEnumerable<Action<ConfigurationBuilder>> configBuilderActions) =>
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(Environment.CurrentDirectory)
                                .AddJsonFile("appsettings.json", true, true);

            if (UserAppsettingFilePath != null)
                configurationBuilder.AddJsonFile(UserAppsettingFilePath, true, true);

            foreach (var action in configBuilderActions)
                action.Invoke(configurationBuilder);

            var configuration = configurationBuilder.Build();

            //var x = configuration.GetDebugView();
            
            return configuration;


        }).As<IConfiguration>()
          .As<IConfigurationRoot>()
          .SingleInstance();

        var serviceCollection = new ServiceCollection().AddOptions();
        builder.Populate(serviceCollection);


        builder.RegisterType<ConfigurationModel>()
            .AsSelf()
            .InstancePerDependency();

        if (IncludeDefaultUI)
        {
            builder.RegisterAllModels<ConfigurationModule>();
        }
    }
}
