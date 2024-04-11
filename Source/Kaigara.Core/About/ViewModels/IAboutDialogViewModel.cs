using Kaigara.Dialogs.ViewModels;

namespace Kaigara.About.ViewModels;

public interface IAboutDialogViewModel : IDialogViewModel
{
    ApplicationInfo ApplicationInfo { get; }
}