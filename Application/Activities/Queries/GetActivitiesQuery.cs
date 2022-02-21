using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Activities.Queries
{
    public class GetActivitiesQuery : IRequest<Result<List<ActivityDto>>> { }

    public class HandlerGetActivitiesCommand : IRequestHandler<GetActivitiesQuery, Result<List<ActivityDto>>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public HandlerGetActivitiesCommand(
            IDataContext context, 
            IMapper mapper,
            IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<List<ActivityDto>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            var activities = await _context.Activities
                .ProjectTo<ActivityDto>(
                    _mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .ToListAsync(cancellationToken);

            return Result<List<ActivityDto>>.Success(activities);
        }
    }
}