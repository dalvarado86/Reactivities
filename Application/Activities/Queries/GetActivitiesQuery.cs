using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Activities.Queries
{
    public class GetActivitiesQuery : IRequest<Result<List<Activity>>> { }

    public class HandlerGetActivitiesCommand : IRequestHandler<GetActivitiesQuery, Result<List<Activity>>>
    {
        private readonly IDataContext _context;

        public HandlerGetActivitiesCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Activity>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            return Result<List<Activity>>.Success(await _context.Activities.ToListAsync());
        }
    }
}