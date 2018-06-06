using Ddd;

namespace Sample.Domain.People.Commands
{
    public class CreatePersonDomainCommand : IDomainCommand<bool>
    {
        public Name Name { get; set; }
    }
}
