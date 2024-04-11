using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Kaigara.Avalonia.Converters;
using Kaigara.Dialogs.ViewModels;

namespace Kaigara.About.ViewModels;
public class AboutDialogViewModel : DialogViewModel, IAboutDialogViewModel
{
    public AboutDialogViewModel(ApplicationInfo applicationInfo)
    {
        ApplicationInfo = applicationInfo;
        Title = $"About {applicationInfo.ProductName}";
        CanResize = false;
        StartupLocation = WindowStartupLocation.CenterOwner;

        if (applicationInfo.IconUri != null)
        {
            Icon = UriToAssetConverter.Instance.Convert(applicationInfo.IconUri, typeof(Bitmap), null, Thread.CurrentThread.CurrentCulture) as Bitmap;
        }

    }

    public ApplicationInfo ApplicationInfo { get; }

    public string Version => $"Version {ApplicationInfo.Version}";

    public Bitmap? Icon { get; }

    public Task Ok() => Close();
}
