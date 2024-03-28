using Kaigara.Commands;

namespace Kaigara.Dialogs.Commands;
public class ShowDialogCommandDefinitionAttribute : CommandDefinitionAttribute
{
    public ShowDialogCommandDefinitionAttribute(string label)
        : base(label)
    {
    }
}
