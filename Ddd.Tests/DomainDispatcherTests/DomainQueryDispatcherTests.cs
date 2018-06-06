using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Ddd.Autofac;
using Xunit;

namespace Ddd.Tests.DomainDispatcherTests
{
    public class DomainQueryDispatcherTests
    {
        [Fact]
        public async Task ItShouldDispatchQueries()
        {
            var container = AutofacConfiguration.CreateContainer(new List<Assembly> {GetType().Assembly});
            var dispatcher = container.Resolve<IDomainQueryDispatcher>();
            var result = await dispatcher.Dispatch(new MyQuery());
            Assert.Equal("result", result);
        }


        [Fact]
        public async Task ItShouldThrownAnExceptionIfHandlerNotSpecified()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                var container = AutofacConfiguration.CreateContainer(new List<Assembly> {GetType().Assembly});
                var dispatcher = container.Resolve<IDomainQueryDispatcher>();
                await dispatcher.Dispatch(new MyQueryWithNoHandler());
            });
        }


        public class MyQuery : IDomainQuery<string>
        {
            
        }

        public class MyQueryWithNoHandler : IDomainQuery<string>
        {
            
        }

        public class MyQueryHandler : IDomainQueryHandler<MyQuery, string>
        {
            public async Task<string> Handle(MyQuery request, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
                return "result";
            }
        }
    }
}
