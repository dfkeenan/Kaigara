using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Kaigara.SourceGenerators
{
    class GeneratorAttributeDefinition
    {
        private readonly AttributeTargets targets;

        public GeneratorAttributeDefinition(string namespaceName, string name, AttributeTargets targets = AttributeTargets.All, bool inherited = true, bool allowMultiple = true)
        {
            Namespace = namespaceName;
            Name = name;
            this.targets = targets;
            Inherited = inherited;
            AllowMultiple = allowMultiple;

            FullName = $"{Namespace}.{Name}";
        }

        public string Namespace { get; }

        public string Name { get;}
        public bool Inherited { get; }
        public bool AllowMultiple { get; }

        public string Targets 
            => string.Join(" | ", targets.ToString().Split(',').Select(s => $"{nameof(AttributeTargets)}.{s.Trim()}"));
        public List<GeneratorAttributePropertyDefinition> Properties { get; } = new();

        public string FullName { get; }

        public bool HasAttribute(ISymbol symbol)
            => symbol.GetAttributes().Any(ad => ad.AttributeClass!.ToDisplayString() == FullName);
    }

    class GeneratorAttributePropertyDefinition
    {
        public GeneratorAttributePropertyDefinition(string typeName, string name, bool constructor = false)
        {
            Constructor = constructor;
            TypeName = typeName;
            Name = name;
        }

        public bool Constructor { get; }
        public string TypeName { get; }
        public string Name { get; }

        public string ParameterName => Name.Substring(0, 1).ToLower() + Name.Substring(1);
    }
}
