using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Platform;
using Avalonia.Styling;

namespace Kaigara.Avalonia.Controls
{
    [PseudoClasses(":platformWindows", ":platformOSX", ":platformLinux")]
    public class ChromeWindow : Window, IStyleable
    {
        public static readonly StyledProperty<IDataTemplate> TitleBarTemplateProperty = AvaloniaProperty.Register<ChromeWindow, IDataTemplate>(nameof(TitleBarTemplate));

        public IDataTemplate TitleBarTemplate
        {
            get => GetValue(TitleBarTemplateProperty);
            set => SetValue(TitleBarTemplateProperty,value);
        }

        public static readonly StyledProperty<double> IconSizeProperty = AvaloniaProperty.Register<ChromeWindow, double>(nameof(IconSize),16);

        public double IconSize
        {
            get => GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public ChromeWindow()
        {


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                PseudoClasses.Add(":platformWindows");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                PseudoClasses.Add(":platformOSX");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                PseudoClasses.Add(":platformLinux");
            }

            AddHandler<TemplateAppliedEventArgs>(TemplateAppliedEvent, OnTemplateApplied!);
        }

        public ChromeWindow(IWindowImpl impl)
            : base(impl)
        {

        }

        Type IStyleable.StyleKey => typeof(ChromeWindow);

        protected void OnTemplateApplied(object sender, TemplateAppliedEventArgs e)
        {
            var titleBar = e.NameScope.Find<Control>("PART_TitleBar");

            titleBar.PointerPressed += TitleBar_PointerPressed;
            titleBar.DoubleTapped += TitleBar_DoubleTapped;
        }

        private void TitleBar_DoubleTapped(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            BeginMoveDrag(e);
        }
    }
}
