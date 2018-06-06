using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Ddd.Autofac;
using MediatR;
using Xunit;

namespace Ddd.Tests.DomainDispatcherTests
{
    public class DomainCommandDispatcherTests
    {
        [Fact]
        public async Task ItShouldDispatchCommandsWhichDoesntReturnResults()
        {
            var container = AutofacConfiguration.CreateContainer(new List<Assembly> {GetType().Assembly});
            var dispatcher = container.Resolve<IDomainCommandDispatcher>();
            await dispatcher.Dispatch(new MyCommandWithNoResult());
        }

        [Fact]
        public async Task ItShouldDispatchCommandsWhichReturnResults()
        {
            var container = AutofacConfiguration.CreateContainer(new List<Assembly> { GetType().Assembly });
            var dispatcher = container.Resolve<IDomainCommandDispatcher>();
            var result = await dispatcher.Dispatch(new MyCommandWithResult { Ping = "Pong" });
            Assert.Equal("Pong", result);
        }

        [Fact]
        public async Task ItShouldThrowAnErrorWhenNoHanlderFound()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var container = AutofacConfiguration.CreateContainer(new List<Assembly> { GetType().Assembly });
                var dispatcher = container.Resolve<IDomainCommandDispatcher>();
                await dispatcher.Dispatch(new MyCommandWithNoHanlder());
            });
        }

        public class MyCommandWithNoResult : IDomainCommand
        {
            
        }

        public class MyCommandWithResult : IDomainCommand<string>
        {
            public string Ping { get; set; }   
        }

        public class MyCommandWithNoHanlder : IDomainCommand
        {
            
        }

        public class MyCommandHandlers : 
            IDomainCommandHandler<MyCommandWithNoResult>, 
            IDomainCommandHandler<MyCommandWithResult, string>
        {
            public async Task<Unit> Handle(MyCommandWithNoResult request, 
                CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
                return new Unit();
            }

            public async Task<string> Handle(MyCommandWithResult request, 
                CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
                return request.Ping;
            }
        }
    }
}
