using Ddd;

namespace Sample.Domain.People.Commands
{
    public class UpdatePersonDomainCommand : IDomainCommand
    {
        public PersonId PersonId { get; set; }

        public Name Name { get; set; }
    }
}