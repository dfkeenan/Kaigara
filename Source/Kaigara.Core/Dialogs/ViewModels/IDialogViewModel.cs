using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.ViewModels;

namespace Kaigara.Dialogs.ViewModels;
public interface IDialogViewModel : IWindowViewModel
{
}

public interface IDialogViewModel<TResult> : IDialogViewModel
{
}
