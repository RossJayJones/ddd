using System.Threading;
using System.Threading.Tasks;
using Ddd;
using MediatR;
using Sample.Domain.Teams.Commands;

namespace Sample.Domain.Teams.CommandHandlers
{
    public class AddPersonToTeamCommandHandler : IDomainCommandHandler<AddPersonToTeamCommand>
    {
        private readonly IRepository<Person> _person;
        private readonly IRepository<Team> _team;

        public AddPersonToTeamCommandHandler(IRepository<Person> person, IRepository<Team> team)
        {
            _person = person;
            _team = team;
        }

        public async Task<Unit> Handle(AddPersonToTeamCommand request, CancellationToken cancellationToken)
        {
            var person = await _person.Load(request.PersonId);
            var team = await _team.Load(request.TeamId);
            await team.AddPerson(person);
            return new Unit();
        }
    }
}