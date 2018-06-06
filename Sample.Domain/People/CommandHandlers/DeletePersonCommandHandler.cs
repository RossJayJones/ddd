using System.Threading;
using System.Threading.Tasks;
using Ddd;
using MediatR;
using Sample.Domain.People.Commands;
using Sample.Domain.People.Events;

namespace Sample.Domain.People.CommandHandlers
{
    public class DeletePersonCommandHandler : IDomainCommandHandler<DeletePersonCommand>, IDomainEventHandler<PersonDeletedDomainEvent>
    {
        private readonly IRepository<Person> _people;

        public DeletePersonCommandHandler(IRepository<Person> people)
        {
            _people = people;
        }

        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _people.Load(request.PersonId).ConfigureAwait(false);
            person.Delete();
            return new Unit();
        }

        public async Task Handle(PersonDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            var person = await _people.Load(notification.PersonId).ConfigureAwait(false);
            _people.Remove(person);
        }
    }
}
