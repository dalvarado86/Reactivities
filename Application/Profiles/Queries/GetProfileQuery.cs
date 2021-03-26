using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profile = Application.Common.Models.Profile;

namespace Application.Profiles.Queries
{
    public class GetProfileQuery : IRequest<Result<Profile>>
    {
        public string Username { get; set; }
    }

    public class HandlerGetProfileQuery : IRequestHandler<GetProfileQuery, Result<Profile>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public HandlerGetProfileQuery(IDataContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Profile>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .ProjectTo<Profile>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Username == request.Username);
            
            if(user == null)
                return null;

            return Result<Profile>.Success(user);
        }
    }
}