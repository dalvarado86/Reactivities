using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities.Commands
{
    public class UpdateAttendanceCommand : IRequest<Result<Unit>>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }

    public class HandlerUpdateAttendanceCommand : IRequestHandler<UpdateAttendanceCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerUpdateAttendanceCommand(IDataContext context, IUserAccessor userAccessor, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(u => u.ApplicationUser)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (activity == null)
                return null;

            var user = await _userManager.FindByNameAsync(_userAccessor.GetUsername());

            if(user == null) 
                return null;

            var hostUsername = activity.Attendees
                .FirstOrDefault(x => x.IsHost)?.ApplicationUser?.UserName;

            var attendance = activity.Attendees
                .FirstOrDefault(x => string.Equals(x.ApplicationUser.UserName, user.UserName));

            if(attendance != null && string.Equals(hostUsername, user.UserName)) 
               activity.IsCanceled = !activity.IsCanceled;

            if(attendance != null && !string.Equals(hostUsername, user.UserName)) 
                activity.Attendees.Remove(attendance);

             if(attendance == null)
             {
                 attendance = new ActivityAttendee
                 {
                     ApplicationUser = user,
                     Activity = activity,
                     IsHost = false
                 };

                 activity.Attendees.Add(attendance);
             }

             var result = await _context.SaveChangesAsync(cancellationToken) > 0;

             return result ? Result<Unit>.Success(Unit.Value) 
                : Result<Unit>.Failure("Problem updating attendance");
        }
    }
}