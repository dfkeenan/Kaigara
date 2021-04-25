using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace Kaigara.Avalonia.Controls
{
    public class ChromeWindowButtons : TemplatedControl
    {
        public ChromeWindowButtons()
        {
            AddHandler<TemplateAppliedEventArgs>(TemplateAppliedEvent, OnTemplateApplied!);
        }

        protected void OnTemplateApplied(object sender, TemplateAppliedEventArgs e)
        {

            if (VisualRoot is Window window)
            {
                var min = e.NameScope.Find<Button>("PART_MinimizeButton");
                min.Click += (s, e) => window.WindowState = WindowState.Minimized;

                var max = e.NameScope.Find<Button>("PART_MaximizeButton");
                max.Click += (s, e) => window.WindowState = WindowState.Maximized;

                var res = e.NameScope.Find<Button>("PART_RestoreButton");
                res.Click += (s, e) => window.WindowState = WindowState.Normal;

                var close = e.NameScope.Find<Button>("PART_CloseButton");
                close.Click += (s, e) => window.Close();

                DoubleTapped += (s, e) => ToggleWindowState(window);

                max.IsVisible = window.WindowState != WindowState.Maximized;
                res.IsVisible = window.WindowState == WindowState.Maximized;

                var windowStateChanged = window.GetPropertyChangedObservable(Window.WindowStateProperty)
                    .Select(e => (WindowState)e.NewValue!);

                max.Bind(Button.IsVisibleProperty, windowStateChanged.Select(ws => ws != WindowState.Maximized));
                res.Bind(Button.IsVisibleProperty, windowStateChanged.Select(ws => ws == WindowState.Maximized));
            }
        }

        private static void ToggleWindowState(Window window)
        {
            switch (window.WindowState)
            {
                case WindowState.Maximized:
                    window.WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    window.WindowState = WindowState.Maximized;
                    break;
            }
        }
    }
}
