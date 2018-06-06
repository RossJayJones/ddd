using System.Collections.Generic;
using Ddd;

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
    }
}
