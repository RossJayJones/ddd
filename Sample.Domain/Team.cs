using System.Collections.Generic;
using System.Threading.Tasks;
using Ddd;
using Sample.Domain.Exceptions;
using Sample.Domain.Queries;

namespace Sample.Domain
{
    public class TeamId : Identity
    {
        public TeamId(string value) : base(value)
        {
        }
    }

    public class Team : Aggregate<TeamId>
    {
        private readonly List<PersonId> _peopleIds;

        public Team()
        {
            _peopleIds = new List<PersonId>();
        }

        public IReadOnlyCollection<PersonId> PeopleIds => _peopleIds;

        public async Task AddPerson(Person person)
        {
            if (_peopleIds.Contains(person.Id))
            {
                return;
            }

            if (person.BelongsToTeam())
            {
                var otherTeamName = await DomainQueryDispatcher.Execute(new FindTeamNameById {TeamId = person.TeamId});

                throw new DomainException($"{person.Name.Full} already belongs to team {otherTeamName}");
            }

            _peopleIds.Add(person.Id);
        }

        public void RemovePerson(Person person)
        {
            if (!_peopleIds.Contains(person.Id))
            {
                return;
            }

            _peopleIds.Remove(person.Id);
        }
    }
}