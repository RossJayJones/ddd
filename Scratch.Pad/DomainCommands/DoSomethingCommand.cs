using Ddd;

namespace Scratch.Pad.DomainCommands
{
    public class DoSomethingCommand : IDomainCommand
    {
        public Person Person { get; }

        public DoSomethingCommand(Person person)
        {
            Person = person;
        }
    }
}
