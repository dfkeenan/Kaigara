using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Commands;

namespace Kaigara.Dialogs.Commands;
public class ShowDialogCommandDefinitionAttribute : CommandDefinitionAttribute
{
    public ShowDialogCommandDefinitionAttribute(string label) 
        : base(label)
    {
    }
}
