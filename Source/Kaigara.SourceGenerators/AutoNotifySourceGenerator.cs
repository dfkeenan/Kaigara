using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;


namespace Kaigara.SourceGenerators
{
    [Generator]
    public class AutoNotifySourceGenerator : ISourceGenerator
    {
        private readonly GeneratorAttributeDefinition autoNotifyAttribute;
        private readonly GeneratorAttributeDefinition autoNotifyAllAttribute;
        private readonly Template? attributeTemplate;

        public AutoNotifySourceGenerator()
        {
            autoNotifyAttribute = new GeneratorAttributeDefinition("AutoNotify", "AutoNotifyAttribute", AttributeTargets.Field, false, false)
            {
                Properties =
                {
                    new ("string","PropertyName", false)
                }
            };

            autoNotifyAllAttribute = new GeneratorAttributeDefinition("AutoNotify", "AutoNotifyAllAttribute", AttributeTargets.Class, false, false)
            {
                
            };

            attributeTemplate = TemplateHelper.LoadTemplate("AttributeTempate.sbn-cs");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Register the attribute source
            context.RegisterForPostInitialization((i) =>
            {
                AddAttribute(i, autoNotifyAttribute);
                AddAttribute(i, autoNotifyAllAttribute);
            });

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver(autoNotifyAttribute, autoNotifyAllAttribute));
        }

        private void AddAttribute(GeneratorPostInitializationContext context, GeneratorAttributeDefinition attributeDefinition)
        {
            if (attributeTemplate is { })
            {
                string source = attributeTemplate.Render(attributeDefinition);
                context.AddSource(attributeDefinition.Name, source);
            }
        }
        
        public void Execute(GeneratorExecutionContext context)
        {
            // retrieve the populated receiver 
            if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
                return;

            // get the added attribute, and INotifyPropertyChanged
            INamedTypeSymbol autoNotifyAttributeSymbol = context.Compilation.GetTypeByMetadataName(autoNotifyAttribute.FullName)!;
            INamedTypeSymbol autoNotifyAllAttributeSymbol = context.Compilation.GetTypeByMetadataName(autoNotifyAllAttribute.FullName)!;
            INamedTypeSymbol notifySymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged")!;

            // group the fields by class, and generate the source
#pragma warning disable RS1024 // Compare symbols correctly
            foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in receiver.Fields.GroupBy(f => f.ContainingType))
#pragma warning restore RS1024 // Compare symbols correctly
            {
                
                //string classSource = ProcessClass(group.Key, group.ToList(), autoNotifyAttributeSymbol, notifySymbol, context);
                context.AddSource($"{group.Key.Name}_autoNotify.cs", SourceText.From($"//{group.Key.Name}", Encoding.UTF8));
            }
        }

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        class SyntaxReceiver : ISyntaxContextReceiver
        {
            private GeneratorAttributeDefinition autoNotifyAttribute;
            private GeneratorAttributeDefinition autoNotifyAllAttribute;

            public SyntaxReceiver(GeneratorAttributeDefinition autoNotifyAttribute, GeneratorAttributeDefinition autoNotifyAllAttribute)
            {
                this.autoNotifyAttribute = autoNotifyAttribute;
                this.autoNotifyAllAttribute = autoNotifyAllAttribute;
            }

            public List<IFieldSymbol> Fields { get; } = new List<IFieldSymbol>();

            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                // any field with at least one attribute is a candidate for property generation
                if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax)
                {
                    foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
                    {
                        // Get the symbol being declared by the field, and keep it if its annotated
                        IFieldSymbol? fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
                        if (fieldSymbol is { })
                        {
                            if (autoNotifyAttribute.HasAttribute(fieldSymbol))
                            {
                                Fields.Add(fieldSymbol);
                                continue;
                            }

                            if(fieldSymbol.ContainingType is { } && autoNotifyAllAttribute.HasAttribute(fieldSymbol.ContainingType))
                            {
                                Fields.Add(fieldSymbol);
                            }
                        }
                    }
                }
            }
        }
    }
}
