using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.ReactiveUI.Controls;
using ExampleApplication.Commands;
using Kaigara.Menus;
using ReactiveUI;

namespace ExampleApplication.Documents.ViewModels
{
    public class ExampleDocumentViewModel : Document
    {
        public ExampleDocumentViewModel(IMenuManager menuManager)
        {
            Id = Guid.NewGuid().ToString();
            Title = "Example Document";
        }

        private bool ischecked;

        public bool IsChecked
        {
            get { return ischecked; }
            set { this.RaiseAndSetIfChanged(ref ischecked, value); }
        }

    }
}
