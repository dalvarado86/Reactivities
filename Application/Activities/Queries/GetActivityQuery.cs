using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Activities.Queries
{
    public class GetActivityQuery : IRequest<Activity>
    {
        public Guid Id { get; set; }
    }

    public class HandlerGetActivityCommand : IRequestHandler<GetActivityQuery, Activity>
    {
        private readonly IDataContext _context;
        public HandlerGetActivityCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<Activity> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
           return await _context.Activities.FindAsync(request.Id);
        }
    }
}