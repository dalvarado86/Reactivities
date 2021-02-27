using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Activities.Commands
{
    public class DeleteActivityCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class HandlerDeleteActivityCommand : IRequestHandler<DeleteActivityCommand>
    {
        private readonly IDataContext _context;

        public HandlerDeleteActivityCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            _context.Activities.Remove(activity);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
                return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}