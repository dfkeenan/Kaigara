using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
