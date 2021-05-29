using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaigara.Commands
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false, Inherited = false)]
    public class CommandDefinitionAttribute : Attribute
    {
        public CommandDefinitionAttribute(string label)
        {
            Label = label;
        }

        public string Label { get; private set; }

        public string? Name { get; set; }
        public string? DefaultInputGesture { get; set; }
    }
}
