using System.Threading;
using System.Threading.Tasks;
using Ddd;
using Sample.Domain.People.Commands;

namespace Sample.Domain.People.CommandHandlers
{
    public class CreatePersonCommandHandler : IDomainCommandHandler<CreatePersonCommand, bool>
    {
        private readonly IRepository<Person> _people;

        public CreatePersonCommandHandler(IRepository<Person> people)
        {
            _people = people;
        }

        public async Task<bool> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
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
