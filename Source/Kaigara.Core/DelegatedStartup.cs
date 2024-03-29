﻿using Autofac;
using Kaigara.Shell;
using Microsoft.Extensions.Configuration;

namespace Kaigara;

public delegate void StartupDelegate(IShell shell, IConfiguration configuration, IContainer container);

internal sealed class DelegatedStartup : Startup
{
    private readonly StartupDelegate startup;
    public DelegatedStartup(IShell shell, IConfiguration configuration, IContainer container, StartupDelegate startup)
        : base(shell, configuration, container)
    {
        this.startup = startup ?? throw new ArgumentNullException(nameof(startup));
    }


    public override void Start()
    {
        startup.Invoke(Shell, Configuration, Container);
    }
}
