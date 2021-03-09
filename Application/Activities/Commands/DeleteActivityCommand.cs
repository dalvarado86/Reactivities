using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Activities.Commands
{
    public class DeleteActivityCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class HandlerDeleteActivityCommand : IRequestHandler<DeleteActivityCommand, Result<Unit>>
    {
        private readonly IDataContext _context;

        public HandlerDeleteActivityCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            //if(activity == null)
            //    return null;

            _context.Activities.Remove(activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to delete the activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}