using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities.Queries
{
    public class GetActivityQuery : IRequest<Result<ActivityDto>>
    {
        public Guid Id { get; set; }
    }

    public class HandlerGetActivityCommand : IRequestHandler<GetActivityQuery, Result<ActivityDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public HandlerGetActivityCommand(
            IDataContext context, 
            IMapper mapper,
            IUserAccessor userAccessor)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .ProjectTo<ActivityDto>(
                    _mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return Result<ActivityDto>.Success(activity);
        }
    }
}