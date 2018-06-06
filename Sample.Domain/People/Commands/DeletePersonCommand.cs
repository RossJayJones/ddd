using Ddd;

namespace Sample.Domain.People.Commands
{
    public class DeletePersonCommand : IDomainCommand
    {
        public PersonId PersonId { get; set; }
    }
}
