﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Kaigara.ToolBars
{
    public sealed class ToolBarModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ToolBarManager>()
                .As<IToolBarManager>()
                .SingleInstance();

        }
    }
}
