using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.ReactiveUI.Controls;
using ExampleApplication.Commands;
using Kaigara.Menus;

namespace ExampleApplication.Documents.ViewModels
{
    public class ExampleDocumentViewModel : Document
    {
        public ExampleDocumentViewModel(IMenuManager menuManager)
        {
            Id = Guid.NewGuid().ToString();
            Title = "Example Document";

            MenuItemDefinition exampleDefinition = new MenuItemDefinition("Example", "_Example")
            {
                new MenuItemDefinition<ExampleCommand>("Thing1"),
                new MenuItemDefinition("Thing2", "Thing _2"),
                new MenuItemDefinition("Thing3", "Thing _3"),
            };
            menuManager.Register(new MenuPath("MainMenu/File"), exampleDefinition);
        }
    }
}
