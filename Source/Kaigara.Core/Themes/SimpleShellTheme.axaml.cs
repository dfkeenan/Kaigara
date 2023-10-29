using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Kaigara.Themes;
public partial class SimpleShellTheme : Styles
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleShellTheme"/> class.
    /// </summary>
    /// <param name="sp">The parent's service provider.</param>
    public SimpleShellTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}