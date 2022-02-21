using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Followers.Commands
{
    public class CreateFollowerCommand : IRequest<Result<Unit>>
    {
        public string TargetUserName { get; set; }
    }

    public class HandlerCreateFollowerCommand : IRequestHandler<CreateFollowerCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerCreateFollowerCommand(
            IDataContext context,
            IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(CreateFollowerCommand request, CancellationToken cancellationToken)
        {
            var observer = await _userManager.FindByNameAsync(_userAccessor.GetUsername());
            var target = await _userManager.FindByNameAsync(request.TargetUserName);

            if (target is null)
            {
                return null;
            }

            var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following is null)
            {
                following = new UserFollowing 
                {
                    Observer = observer,
                    Target = target,
                };

                _context.UserFollowings.Add(following);
            }
            else
            {
                _context.UserFollowings.Remove(following);
            }

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!success)
            {
                return Result<Unit>.Failure("Failed to update following");
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}