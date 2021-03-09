using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Activities.Queries
{
    public class GetActivityQuery : IRequest<Result<Activity>>
    {
        public Guid Id { get; set; }
    }

    public class HandlerGetActivityCommand : IRequestHandler<GetActivityQuery, Result<Activity>>
    {
        private readonly IDataContext _context;
        public HandlerGetActivityCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<Activity>> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
           var activity = await _context.Activities.FindAsync(request.Id);

           return Result<Activity>.Success(activity);
        }
    }
}