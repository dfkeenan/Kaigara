using Autofac;

namespace Kaigara
{
    public static class AutofacExtensions
    {
        public static T CreateInstance<T>(this IComponentContext context)
            where T : notnull
        {
            return context.Resolve<ILifetimeScope>().CreateInstance<T>();
        }

        public static T CreateInstance<T>(this ILifetimeScope scope)
            where T : notnull
        {           
            using var innerScope = scope.BeginLifetimeScope(b => b.RegisterType<T>().ExternallyOwned());

            return innerScope.Resolve<T>();
        }

        public static Activator<T> GetActivator<T>(this IComponentContext context)
           where T : notnull
        {
            return context.Resolve<ILifetimeScope>().GetActivator<T>();
        }

        public static Activator<T> GetActivator<T>(this ILifetimeScope scope)
            where T : notnull
        {
            return new Activator<T>(scope);
        }
    }

    public class Activator<TResult>
        where TResult : notnull
    {
        private readonly ILifetimeScope lifetimeScope;

        public Activator(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
        }

        public TResult Create<T>(T arg)
        {
            using var innerScope = GetInnerScope();

            return innerScope.Resolve<Func<T, TResult>>().Invoke(arg);
        }

        private ILifetimeScope GetInnerScope()
        {
            return lifetimeScope.BeginLifetimeScope(b => b.RegisterType<TResult>().ExternallyOwned());
        }
    }
}
