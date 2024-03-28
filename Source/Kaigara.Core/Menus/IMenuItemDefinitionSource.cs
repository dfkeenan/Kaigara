using System.Diagnostics.CodeAnalysis;
using Kaigara.Commands;

namespace Kaigara.Menus;
public interface IMenuItemDefinitionSource
{
    bool IsDefined { get; }

    [MemberNotNullWhen(returnValue: true, nameof(IsDefined))]
    MenuItemLocation? Location { get; }

    [MemberNotNullWhen(returnValue: true, nameof(IsDefined))]
    MenuItemDefinition? GetDefinition(RegisteredCommandBase? command);
}
