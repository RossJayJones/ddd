using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ddd;
using MediatR;
using Scratch.Pad.DomainCommands;

namespace Scratch.Pad.DomainHandlers
{
    public class DoSomethingCommandHandler : IDomainCommandHandler<DoSomethingCommand>
    {
        private readonly IRepository<Person> _people;

        public DoSomethingCommandHandler(IRepository<Person> people)
        {
            _people = people;
        }

        public async Task<Unit> Handle(DoSomethingCommand request, CancellationToken cancellationToken)
        {
            var person = await _people.Load(new PersonId("person2"));
            person.ChangeName("bla", "bla");
            return new Unit();
        }
    }
}
