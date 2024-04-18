using Kaigara.Collections.Generic;
using Kaigara.Configuration.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Kaigara.Configuration;
public class ConfigurationManager
{
    private readonly IConfigurationRoot configuration;
    private readonly string? userSettingFilePath;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public ConfigurationManager(IConfigurationRoot configuration, string? UserSettingFilePath)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.userSettingFilePath = UserSettingFilePath;

        jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IgnoreReadOnlyProperties = true,
            Converters =
            {
                new OptionsPageViewModelConverterFactory()
            }
        };
    }

    public void BindSection(Type sectionType, object section) 
    {
        ArgumentNullException.ThrowIfNull(sectionType);
        ArgumentNullException.ThrowIfNull(section);

        configuration.GetSection(sectionType.FullName!)?.Bind(section);
    }

    public async Task<bool> UpdateAsync<T>(IEnumerable<(Type SectionType, T Section)> updates, CancellationToken cancellationToken = default)
        where T : class
    {

        if (string.IsNullOrWhiteSpace(userSettingFilePath))
        {
            return false;
        }

        JsonObject? config = null;
        {
            using var file = File.OpenRead(userSettingFilePath);
            using var configDocument = await JsonDocument.ParseAsync(file, cancellationToken: cancellationToken);
            config = configDocument.Deserialize<JsonObject>();

            if (config is null) return false;
        }


        foreach (var (sectionType, section) in updates)
        {
            var jsonNode = JsonSerializer.SerializeToNode(section, section.GetType(), jsonSerializerOptions);
            config[sectionType.FullName!] = jsonNode;
        }

        {
            using var file = File.Open(userSettingFilePath, FileMode.Create);
            await JsonSerializer.SerializeAsync(file, config, jsonSerializerOptions, cancellationToken: cancellationToken);
        }

        //configuration.Reload();

        return true;
    }

    

    private class OptionsPageViewModelConverterFactory : JsonConverterFactory
    {
        private readonly Dictionary<Type, Converter> converters = new();

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(OptionsPageViewModel));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return converters.GetOrAdd(typeToConvert, t => new Converter(t));
        }

        class Converter : JsonConverter<OptionsPageViewModel>
        {
            private readonly List<PropertyInfo> properties = new();
            public Converter(Type typeToConvert)
            {
                var modelProperties = typeToConvert.GetCustomAttribute<OptionsPageAttribute>()!
                    .ModelType.GetProperties().ToList();

                foreach (var property in typeToConvert.GetProperties())
                {
                    if (!(property.SetMethod?.IsPublic ?? false)) continue;
                    var i = modelProperties.FindIndex(p => p.Name == property.Name && p.PropertyType == property.PropertyType);
                    if(i == -1) continue;
                    modelProperties.RemoveAt(i);
                    properties.Add(property);
                }

            }

            public override OptionsPageViewModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, OptionsPageViewModel value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                foreach (var property in properties)
                {
                    writer.WritePropertyName(property.Name);
                    JsonSerializer.Serialize(writer, property.GetValue(value), property.PropertyType,  options);
                }
                writer.WriteEndObject();
            }
        }
    }
}
