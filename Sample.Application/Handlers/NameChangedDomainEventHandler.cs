using System.Threading;
using System.Threading.Tasks;
using Ddd;
using Sample.Application.InfrastructureServices;
using Sample.Domain;
using Sample.Domain.People.Events;

namespace Sample.Application.Handlers
{
    public class NameChangedDomainEventHandler : IDomainEventHandler<NameChangedDomainEvent>
    {
        private readonly IRepository<Person> _people;
        private readonly IEmailDispatcherService _emailDispatcherService;

        public NameChangedDomainEventHandler(IRepository<Person> people, IEmailDispatcherService emailDispatcherService)
        {
            _people = people;
            _emailDispatcherService = emailDispatcherService;
        }

        public async Task Handle(NameChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var person = await _people.Load(notification.PersonId);

            foreach (var contact in person.Contacts)
            {
                await _emailDispatcherService.Dispatch(contact.Email, $"Dear {person.Name.Full}, your account has been modified");
            }
        }
    }
}
