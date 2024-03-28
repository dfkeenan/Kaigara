using System.Diagnostics.CodeAnalysis;
using Kaigara.Commands;

namespace Kaigara.Toolbars;

public interface IToolbarItemDefinitionSource
{
    bool IsDefined { get; }
   
    [MemberNotNullWhen(returnValue: true, nameof(IsDefined))]
    ToolbarItemLocation? Location { get; }

    [MemberNotNullWhen(returnValue: true, nameof(IsDefined))]
    ToolbarItemDefinition? GetDefinition(RegisteredCommandBase? command);
}