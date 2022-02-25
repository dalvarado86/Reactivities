using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles.Queries
{
    public class GetActivitiesQuery : IRequest<Result<List<UserActivityDto>>>
    {
        public string Username { get; set; }
        public string Predicate { get; set; }
    }

    public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, Result<List<UserActivityDto>>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetActivitiesQueryHandler(IDataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<UserActivityDto>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ActivityAttendees
                .Where(u => u.ApplicationUser.UserName == request.Username)
                .OrderBy(a => a.Activity.Date)
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = request.Predicate switch
            {
                "past" => query.Where(a => a.Date <= DateTime.Now),
                "hosting" => query.Where(a => a.HostUsername == request.Username),
                _ => query.Where(a => a.Date >= DateTime.Now)
            };

            var activities = await query.ToListAsync();

            return Result<List<UserActivityDto>>.Success(activities);
        }
    }
}