using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using SkiaSharp;


if (args is not [string inputPath, string outputFilePath, string backgroundColorHex] 
    || Directory.Exists(inputPath) is false
    || SKColor.TryParse(backgroundColorHex, out var backgroundColor) is false) return;


var icons = from filePath in Directory.EnumerateFiles(inputPath, "*.xaml", SearchOption.AllDirectories)
            let content = XDocument.Load(filePath)
            let name = Path.GetFileNameWithoutExtension(filePath)
            select (name, content);

//icons = icons.Take(10);

var avons = "https://github.com/avaloniaui";
var xns = "http://schemas.microsoft.com/winfx/2006/xaml";
var nameAttr = XName.Get("Name", xns);
var keyAttr = XName.Get("Key", xns);

var drawings = new List<XElement>();
var resources = new HashSet<XElement>(new XNodeEqualityComparer());
var resourceKeys = new HashSet<string>();

foreach (var (name, content) in icons)
{
    if (content.Root is not XElement root) continue;

    ChangeNamespace(root, avons);

    if(GetIconDrawing(root, name) is not XElement drawing) continue;
    drawings.Add(drawing);

    var drawingResources= root.Descendants().FirstOrDefault(e => e.Name.LocalName == "Rectangle.Resources")?.Elements();

    if(drawingResources is null) continue;

    foreach (var resource in drawingResources)
    {
        var key = resource.Attribute(keyAttr)!;

        if (!resources.Contains(resource) && resourceKeys.Contains(key.Value))
        {
            //WTF duplicate key
            int n = 1;
            string wtfKey;
            do
            {
                wtfKey = $"{key.Value}-WTF{n++}";
            }
            while (resourceKeys.Contains(wtfKey));

            var resourceSelectors = drawing.Descendants()
                .SelectMany(e => e.Attributes())
                .Where(a => a.Value.Contains(key.Value));

            foreach (var resourceSelector in resourceSelectors)
            {
                resourceSelector.Value = resourceSelector.Value.Replace(key.Value, wtfKey);
            }

            key.Value = wtfKey;
        }

        resources.Add(resource);
        resourceKeys.Add(key.Value);
    }

}


var result = CreateResourceDictionary(drawings, resources);

Save(result, outputFilePath);

//Console.ReadKey();


void ChangeNamespace(XElement element, XNamespace ns)
{
    var dns = element.GetDefaultNamespace();

    foreach(var el in element.DescendantsAndSelf())
    {
        if(el.Name.Namespace == dns)
        {
            el.Name = ns.GetName(el.Name.LocalName);
            var attr = el.Attributes().ToList();
            el.Attributes().Remove();

            foreach (var at in attr)
            {
                if(at.Name.Namespace == dns)
                {
                    el.Add(new XAttribute(ns.GetName(at.Name.LocalName), at.Value));
                }
                else
                {
                    el.Add(at);
                }
            }

        }
    }

    var defaultNamespace = element.Attributes().First(a => a.Name.LocalName == "xmlns");
    //defaultNamespace.Remove();
    defaultNamespace.Value = "https://github.com/avaloniaui";
}

XElement? GetIconDrawing(XElement element, string key)
{
    var drawing = element.Descendants().FirstOrDefault(e => e.Name.LocalName == "DrawingBrush.Drawing")?.Elements();

    if (drawing is null) return null;

    foreach (var el in drawing.Descendants())
    {
        el.Attribute(nameAttr)?.Remove();
    }

    var result =  new XElement(XName.Get("DrawingImage", avons), drawing);
    result.Add(new XAttribute(keyAttr, key));   


    return result;
}

XDocument CreateResourceDictionary(IEnumerable<XElement> drawings, IEnumerable<XElement> resources)
{
    var doc = XDocument.Parse(
        """
        <ResourceDictionary xmlns="https://github.com/avaloniaui"
                            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                            xmlns:System="clr-namespace:System;assembly=mscorlib">
            <!-- Add Resources Here -->
          <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Default">
            </ResourceDictionary>
            <ResourceDictionary x:Key="Dark">
            </ResourceDictionary>
          </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
        """
        );

    var themes = doc.Root!.Descendants().Where(e => e.Name.LocalName == "ResourceDictionary");

    themes.First().Add(resources);
    themes.Skip(1).First().Add(ChangeThemeColors(resources));

    var darkDictionary = themes.Skip(1).First();

    doc.Root!.Add(drawings);
    return doc;
}

IEnumerable<XElement> ChangeThemeColors(IEnumerable<XElement> resource)
{
    backgroundColor.ToHsl(out var _, out var _, out var backgroundLuminosity);


    foreach ( var e in resource)
    {
        var clone = new XElement(e);

        foreach ( var colorAttr in clone.DescendantsAndSelf().SelectMany(e => e.Attributes().Where(a => a.Name.LocalName == "Color")))
        {
            if (SKColor.TryParse(colorAttr.Value, out var color) is false) continue;

            color.ToHsl(out var h, out var s, out var l);

            l = (float)SKTransformLuminosity(h, s, l, backgroundLuminosity);

            colorAttr.Value = SKColor.FromHsl(h, s, l, color.Alpha).ToString();
        }


        yield return clone;
    }

    static double SKTransformLuminosity(double hue, double saturation, double luminosity, double backgroundLuminosity)
            => TransformLuminosity(hue, saturation / 100.0, luminosity / 100.0, backgroundLuminosity / 100.0) * 100.0;

    static double TransformLuminosity(double hue, double saturation, double luminosity, double backgroundLuminosity)
    {
        if (backgroundLuminosity < 0.5)
        {
            if (luminosity >= 82.0 / 85.0)
                return backgroundLuminosity * (luminosity - 1.0) / (-3.0 / 85.0);
            double val2 = saturation >= 0.2 ? (saturation <= 0.3 ? 1.0 - (saturation - 0.2) / (1.0 / 10.0) : 0.0) : 1.0;
            double num1 = Math.Max(Math.Min(1.0, Math.Abs(hue - 37.0) / 20.0), val2);
            double num2 = ((backgroundLuminosity - 1.0) * 0.66 / (82.0 / 85.0) + 1.0) * num1 + 0.66 * (1.0 - num1);
            if (luminosity < 0.66)
                return (num2 - 1.0) / 0.66 * luminosity + 1.0;
            return (num2 - backgroundLuminosity) / (-259.0 / 850.0) * (luminosity - 82.0 / 85.0) + backgroundLuminosity;
        }
        if (luminosity < 82.0 / 85.0)
            return luminosity * backgroundLuminosity / (82.0 / 85.0);
        return (1.0 - backgroundLuminosity) * (luminosity - 1.0) / (3.0 / 85.0) + 1.0;
    }
}

static void Save(XDocument document, string? path)
{
    var settings = new XmlWriterSettings();
    settings.OmitXmlDeclaration = true;
    settings.Indent = true;
    settings.NewLineOnAttributes = true;

    if (path != null)
    {
        using var stream = File.Open(path, FileMode.Create);
        using var writer = XmlWriter.Create(stream, settings);
        document.Save(writer);
    }
    else
    {
        var builder = new StringBuilder();
        using var writer = XmlWriter.Create(builder, settings);
        document.Save(writer);
        writer.Flush();
        var result = builder.ToString();
        Console.WriteLine(result);
    }

}