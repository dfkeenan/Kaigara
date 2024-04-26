using System.Diagnostics.CodeAnalysis;
using Kaigara.Commands;

namespace Kaigara.Menus;
public interface IMenuItemDefinitionSource
{
    [MemberNotNullWhen(returnValue: true, nameof(Location))]
    bool IsDefined { get; }
    MenuItemLocation? Location { get; }
    MenuItemDefinition? GetDefinition(RegisteredCommandBase? command);
}
