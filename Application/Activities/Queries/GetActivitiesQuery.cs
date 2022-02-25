using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Application.Common.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace Application.Activities.Queries
{
    public class GetActivitiesQuery : IRequest<Result<PagedList<ActivityDto>>> 
    { 
        public ActivityParams Params { get; set; }
    }

    public class HandlerGetActivitiesCommand : IRequestHandler<GetActivitiesQuery, Result<PagedList<ActivityDto>>>
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

        public async Task<Result<PagedList<ActivityDto>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Activities
                .Where(d => d.Date >= request.Params.StartDate)
                .OrderBy(d => d.Date)
                .ProjectTo<ActivityDto>(
                    _mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .AsQueryable();

            if (request.Params.IsGoing && !request.Params.IsHost) 
            {
                query = query
                    .Where(x => x.Attendees
                        .Any(a => a.Username == _userAccessor.GetUsername()));
            }

            if (request.Params.IsHost && !request.Params.IsGoing) 
            {
                query = query.Where(x => x.HostUserName == _userAccessor.GetUsername());
            }

            return Result<PagedList<ActivityDto>>.Success(
                await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
            );
        }
    }
}