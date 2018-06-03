using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ddd;
using MediatR;
using Scratch.Pad.DomainCommands;

namespace Scratch.Pad.DomainHandlers
{
    public class DoSomethingCommandHandler : IDomainCommandHandler<DoSomethingCommand>
    {
        public Task<Unit> Handle(DoSomethingCommand request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
