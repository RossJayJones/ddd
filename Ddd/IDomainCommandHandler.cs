using MediatR;

namespace Ddd
{
    public interface IDomainCommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : IDomainCommand
    {
    }
}
