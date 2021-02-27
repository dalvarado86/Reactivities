using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Activities.Commands
{
    public class CreateActivityCommand : IRequest
    {
        public Activity Activity { get; set; }
    }

    public class HandlerCreateActivityCommand : IRequestHandler<CreateActivityCommand>
    {
        private readonly IDataContext _context;

        public HandlerCreateActivityCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Activity);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
                return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }

}