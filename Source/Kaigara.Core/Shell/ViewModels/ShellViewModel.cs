using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Core;
using Kaigara.ViewModels;
using ReactiveUI;

namespace Kaigara.Shell.ViewModels
{
    public class ShellViewModel : ViewModel, IShell
    {
        private IFactory factory;
        public IFactory Factory
        {
            get => factory;
            set => this.RaiseAndSetIfChanged(ref factory, value);
        }

        private IDock layout;

        public IDock Layout
        {
            get => layout;
            set => this.RaiseAndSetIfChanged(ref layout, value);
        }

        public ShellViewModel(IFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            layout = factory.CreateLayout()!;
            factory.InitLayout(layout);
        }
    }
}
