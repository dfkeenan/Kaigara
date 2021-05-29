using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;

namespace Kaigara.Commands
{
    public abstract class RegisteredCommandBase : ReactiveObject
    {
        public RegisteredCommandBase()
        {
            var thisType = GetType();
            Name = thisType.FullName!;
            label = thisType.Name;

            if(thisType.GetCustomAttribute<CommandDefinitionAttribute>() is CommandDefinitionAttribute attribute)
            {
                if(attribute.DefaultInputGesture is string gesture)
                {
                    keyGesture = KeyGesture.Parse(gesture);
                }

                Name = attribute.Name ?? Name;
                Label = attribute.Label;
            }
            Command = CreateCommand();
        }

        protected RegisteredCommandBase(string name, string label, KeyGesture? keyGesture)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.label = label ?? throw new ArgumentNullException(nameof(label));
            this.keyGesture = keyGesture;
            Command = CreateCommand();
        }

        public string Name { get; }

        private string label;
        public string Label
        {
            get => label;
            set => this.RaiseAndSetIfChanged(ref label, value);
        }

        private KeyGesture? keyGesture;

        public KeyGesture? InputGesture
        {
            get => keyGesture;
            set => this.RaiseAndSetIfChanged(ref keyGesture,value);
        }

        public ICommand Command { get; }

        protected virtual IObservable<bool>? CanExecute => null;

        protected abstract ICommand CreateCommand();
    }
}
