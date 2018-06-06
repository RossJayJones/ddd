using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core.Lifetime;
using Ddd;
using Ddd.Autofac;
using Ddd.Behaviours;
using MediatR;
using MediatR.Pipeline;
using Scratch.Pad.Data;
using Scratch.Pad.DomainEvents;
using Scratch.Pad.DomainHandlers;
using Scratch.Pad.DomainQueries;
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
                var person1 = await repository.Load(new PersonId("person1")); //new Person(new PersonId("person2"), "Test", "Person");
                person1.ChangeName("x", "y");
                await person1.DoSomething();
                //var person2 = await repository.Load(new PersonId("person2")); //new Person(new PersonId("person2"), "Test", "Person");
                //await repository.Add(person);
                var people = await DomainQueryDispatcher.Execute(new FindPeople());
                await uow.Commit();
                await db.SaveChangesAsync();
            }
        }


        private static IContainer CreateContainer()
        {
            var assembliesToScan = new List<Assembly> {typeof(Program).GetTypeInfo().Assembly};

            var container = AutofacConfiguration.CreateContainer(assembliesToScan, builder =>
            {
                builder.RegisterType<MyDbContext>().InstancePerLifetimeScope();
                builder.RegisterType<PersonRepository>().As<IRepository<Person>>().As<IRepository<Person>>();
            });

            return container;
        }
    }
}
