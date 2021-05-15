using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;

namespace Kaigara.Shell
{
    internal sealed class ReactiveDockFactory
    {
        public ReactiveDockFactory(IFactory factory)
        {
            this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));


        }

        public IFactory Factory { get; }
    }
}
