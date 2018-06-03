using System;
using Ddd;
using Ddd.Behaviours;
using MediatR;
using Scratch.Pad.Repositories;

namespace Scratch.Pad
{
    class Program
    {
        static void Main(string[] args)
        {
            IMediator mediator = null;

            var domainDispatcher = new DomainDispatcher(mediator);
            var uow = new DomainEventPublisherBehaviour(domainDispatcher);
            var repository = new CustomerRepository(uow);
            var customer = repository.Load(new CustomerId("customer")).Result;
        }
    }
}
