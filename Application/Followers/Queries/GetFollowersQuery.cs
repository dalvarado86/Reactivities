using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Profile = Application.Common.Models.Profile;

namespace Application.Followers.Queries
{
    public class GetFollowersQuery : IRequest<Result<List<Profile>>>
    {
        public string Predicate { get; set; }
        public string Username { get; set; }
    }

    public class HandlerGetFollowersQuery : IRequestHandler<GetFollowersQuery, Result<List<Profile>>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public HandlerGetFollowersQuery(
            IDataContext context, 
            IMapper mapper,
            IUserAccessor userAccessor
            )
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<List<Profile>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
        {
            var profiles = new List<Profile>();

            switch (request.Predicate)
            {
                case "followers" :
                    profiles = await _context.UserFollowings
                        .Where(x => x.Target.UserName == request.Username)
                        .Select(u => u.Observer)
                        .ProjectTo<Profile>(
                            _mapper.ConfigurationProvider, 
                            new { currentUsername = _userAccessor.GetUsername() })
                        .ToListAsync();
                break;
                case "followings" :
                    profiles = await _context.UserFollowings
                        .Where(x => x.Observer.UserName == request.Username)
                        .Select(u => u.Target)
                        .ProjectTo<Profile>(
                            _mapper.ConfigurationProvider,
                            new { currentUsername = _userAccessor.GetUsername() })
                        .ToListAsync();
                break;

            }

            return Result<List<Profile>>.Success(profiles);
        }
    }
}