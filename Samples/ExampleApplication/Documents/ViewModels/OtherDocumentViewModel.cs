using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.ReactiveUI.Controls;

namespace ExampleApplication.Documents.ViewModels
{
    public class OtherDocumentViewModel : Document
    {
        public OtherDocumentViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Title = "Other Document";
        }
    }
}
