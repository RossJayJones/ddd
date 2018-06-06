using Ddd;

namespace Sample.Domain.People.Commands
{
    public class CreatePersonCommand : IDomainCommand<bool>
    {
        public Name Name { get; set; }
    }
}
