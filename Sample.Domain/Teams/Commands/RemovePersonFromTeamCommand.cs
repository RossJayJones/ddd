using Ddd;

namespace Sample.Domain.Teams.Commands
{
    public class RemovePersonFromTeamCommand : IDomainCommand
    {
        public TeamId TeamId { get; set; }

        public PersonId PersonId { get; set; }
    }
}
