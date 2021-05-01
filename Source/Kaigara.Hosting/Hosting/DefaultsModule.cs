using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kaigara.DependencyInjection;

namespace Kaigara.Hosting
{
    public sealed class DefaultsModule : ShellAppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            DependsOnModule<MainWindowModule>(builder);
        }
    }
}
