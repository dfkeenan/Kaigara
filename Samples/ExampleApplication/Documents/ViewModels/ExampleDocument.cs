using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.ReactiveUI.Controls;

namespace ExampleApplication.Documents.ViewModels
{
    public class ExampleDocumentViewModel : Document
    {
        public ExampleDocumentViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Title = "Example Document";
        }
    }
}
