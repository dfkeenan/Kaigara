using Avalonia;
using Avalonia.Input;
using Kaigara.Collections.ObjectModel;
using ReactiveUI;

namespace Kaigara.Commands;

public static class CommandManagerExtensions
{
    public static IDisposable SyncKeyBindings(this ICommandManager commandManager, IList<KeyBinding> keyBindings)
    {
        return Collections.ObjectModel.ReadOnlyObservableCollectionExtensionsHelpers.ToReadOnlyObservableCollectionOf<RegisteredCommandBase, KeyBinding>(commandManager.Commands, (Func<RegisteredCommandBase, KeyBinding>)(c =>
               new KeyBinding()
               {
                   [!KeyBinding.CommandProperty] = c.WhenAnyValue(c => c.Command).ToBinding(),
                   [!KeyBinding.GestureProperty] = c.WhenAnyValue<RegisteredCommandBase, KeyGesture>(c => c.InputGesture).ToBinding(),

               })).SyncTo(keyBindings);
    }
}
