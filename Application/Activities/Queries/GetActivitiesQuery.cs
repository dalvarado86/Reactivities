using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Activities.Queries
{
    public class GetActivitiesQuery : IRequest<List<Activity>> { }

    public class HandlerGetActivitiesCommand : IRequestHandler<GetActivitiesQuery, List<Activity>>
    {
        private readonly IDataContext _context;

        public HandlerGetActivitiesCommand(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Activities.ToListAsync();
        }
    }
}