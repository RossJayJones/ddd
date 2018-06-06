using Ddd;

namespace Sample.Domain.Teams.Commands
{
    public class AddPersonToTeamCommand : IDomainCommand
    {
        public TeamId TeamId { get; set; }

        public PersonId PersonId { get; set; }
    }
}
