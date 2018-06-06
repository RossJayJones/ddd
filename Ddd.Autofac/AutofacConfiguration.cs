using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Autofac;
using Autofac.Core.Lifetime;
using Ddd.Behaviours;
using MediatR;

namespace Ddd.Autofac
{
    public static class AutofacConfiguration
    {
        static readonly IDictionary<ExecutionContext, ILifetimeScope> ExecutingScopes = new Dictionary<ExecutionContext, ILifetimeScope>();
        
        public static IContainer CreateContainer(IEnumerable<Assembly> assembliesToScan, Action<ContainerBuilder> configure = null)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[] { typeof(IRequestHandler<,>), typeof(INotificationHandler<>) };

            foreach (var assembly in assembliesToScan)
            {
                foreach (var mediatrOpenType in mediatrOpenTypes)
                {
                    builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(mediatrOpenType).AsImplementedInterfaces();
                }
            }

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterType<DomainUnitOfWork>().As<IDomainUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DomainEventPublisherBehaviour>().As<IDomainBehaviour>().InstancePerLifetimeScope();
            builder.RegisterType<DomainDispatcher>().AsImplementedInterfaces();

            configure?.Invoke(builder);

            var container = builder.Build();

            DomainCommandDispatcher.Factory = () =>
            {
                var context = ExecutionContext.Capture();
                return ExecutingScopes[context].Resolve<IDomainCommandDispatcher>();
            };

            DomainQueryDispatcher.Factory = () =>
            {
                var context = ExecutionContext.Capture();
                return ExecutingScopes[context].Resolve<IDomainQueryDispatcher>();
            };

            container.ChildLifetimeScopeBeginning += OnScopeStarting;

            return container;
        }

        static void OnScopeStarting(object item, LifetimeScopeBeginningEventArgs args)
        {
            var context = ExecutionContext.Capture();

            if (ExecutingScopes.ContainsKey(context))
            {
                return;
            }

            ExecutingScopes.Add(context, args.LifetimeScope);

            args.LifetimeScope.CurrentScopeEnding += OnScopeEnding;
        }

        static void OnScopeEnding(object item, LifetimeScopeEndingEventArgs args)
        {
            args.LifetimeScope.CurrentScopeEnding -= OnScopeEnding;

            var context = ExecutionContext.Capture();

            if (!ExecutingScopes.ContainsKey(context))
            {
                return;
            }

            ExecutingScopes.Remove(context);
        }
    }
}
