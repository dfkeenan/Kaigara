using Autofac;
using Kaigara.Shell;
using Microsoft.Extensions.Configuration;

namespace Kaigara;

public abstract class Startup
{
    protected Startup(IShell shell, IConfiguration configuration, IContainer container)
    {
        Shell = shell ?? throw new ArgumentNullException(nameof(shell));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public IShell Shell { get; }
    public IConfiguration Configuration { get; }
    public IContainer Container { get; }

    public abstract void Start();
}
