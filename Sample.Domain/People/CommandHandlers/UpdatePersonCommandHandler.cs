using System.Threading;
using System.Threading.Tasks;
using Ddd;
using MediatR;
using Sample.Domain.People.Commands;

namespace Sample.Domain.People.CommandHandlers
{
    public class UpdatePersonCommandHandler : IDomainCommandHandler<UpdatePersonDomainCommand>
    {
        private readonly IRepository<Person> _people;

        public UpdatePersonCommandHandler(IRepository<Person> people)
        {
            _people = people;
        }

        public async Task<Unit> Handle(UpdatePersonDomainCommand request, CancellationToken cancellationToken)
        {
            var person = await _people.Load(request.PersonId);
            person.Update(request.Name);
            return new Unit();
        }
    }
}
