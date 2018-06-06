using System.Threading;
using System.Threading.Tasks;
using Ddd;
using Sample.Domain;
using Sample.Domain.People.Commands;

namespace Sample.Application.Handlers
{
    public class CreatePersonDomainCommandHandler : IDomainCommandHandler<CreatePersonDomainCommand, bool>
    {
        private readonly IRepository<Person> _people;

        public CreatePersonDomainCommandHandler(IRepository<Person> people)
        {
            _people = people;
        }

        public async Task<bool> Handle(CreatePersonDomainCommand request, CancellationToken cancellationToken)
        {
            if (request.Name == null)
            {
                return false;
            }

            var person = new Person(request.Name);
            await _people.Add(person);
            return true;
        }
    }
}
