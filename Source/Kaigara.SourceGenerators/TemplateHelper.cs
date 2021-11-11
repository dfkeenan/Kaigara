using System;
using System.IO;
using System.Linq;
using Scriban;

namespace Kaigara.SourceGenerators;

class TemplateHelper
{
    private static readonly string[] resourceNames;

    static TemplateHelper()
    {
        resourceNames = typeof(TemplateHelper).Assembly.GetManifestResourceNames();
    }

    public static Template? LoadTemplate(string name)
    {
        var resourcePath = resourceNames.FirstOrDefault(n => n.EndsWith(name, StringComparison.OrdinalIgnoreCase));
        if (resourcePath is null)
        {
            return null;
        }

        using Stream stream = typeof(TemplateHelper).Assembly.GetManifestResourceStream(resourcePath);
        using StreamReader reader = new StreamReader(stream);

        var templateSource = reader.ReadToEnd();

        return Template.Parse(templateSource);
    }
}
