using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using ReactiveUI;
//using AutoNotify;

namespace Kaigara.ViewModels
{
    //[AutoNotifyAll]
    public abstract partial class WindowViewModel : ActivatableViewModel, IWindowViewModel
    {
        protected WindowViewModel()
        {
            Width = double.NaN;
            Height = double.NaN;
        }

        private string? title;

        public string? Title
        {
            get { return title; }
            set { this.RaiseAndSetIfChanged(ref title, value); }
        }

        private Uri? iconUri;

        public Uri? IconUri
        {
            get { return iconUri; }
            set { this.RaiseAndSetIfChanged(ref iconUri, value); }
        }


        private WindowStartupLocation startupLocation;

        public WindowStartupLocation StartupLocation
        {
            get { return startupLocation; }
            set { this.RaiseAndSetIfChanged(ref startupLocation, value); }
        }

        private double width;

        public double Width
        {
            get { return width; }
            set { this.RaiseAndSetIfChanged(ref width, value); }
        }

        private double height;

        public double Height
        {
            get { return height; }
            set { this.RaiseAndSetIfChanged(ref height, value); }
        }

    }
}
