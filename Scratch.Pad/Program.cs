using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Ddd;
using Ddd.Behaviours;
using MediatR;
using MediatR.Pipeline;
using Scratch.Pad.Data;
using Scratch.Pad.DomainEvents;
using Scratch.Pad.DomainHandlers;
using Scratch.Pad.Repositories;
namespace Scratch.Pad
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {

            var container = CreateContainer();

            using (var scope = container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IDomainUnitOfWork>();
                var db = scope.Resolve<MyDbContext>();
                var repository = scope.Resolve<IRepository<Person>>();
                var person = new Person(new PersonId("person1"), "Test", "Person");
                await repository.Add(person);
                await uow.Commit();
                await db.SaveChangesAsync();
            }
        }


        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[] { typeof(IRequestHandler<,>), typeof(INotificationHandler<>) };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder.RegisterAssemblyTypes(typeof(Program).GetTypeInfo().Assembly).AsClosedTypesOf(mediatrOpenType).AsImplementedInterfaces();
            }

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterType<MyDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<DomainUnitOfWork>().As<IDomainUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DomainEventPublisherBehaviour>().As<IDomainBehaviour>().InstancePerLifetimeScope();
            builder.RegisterType<PersonRepository>().As<IRepository<Person, PersonId>>().As<IRepository<Person>>();
            builder.RegisterType<DomainDispatcher>().AsImplementedInterfaces();

            var container = builder.Build();

            return container;
        }
    }
}
