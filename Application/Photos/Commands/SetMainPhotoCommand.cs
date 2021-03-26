using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Photos.Commands
{
    public class SetMainPhotoCommand : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }

    public class HandlerSetMainPhotoCommand : IRequestHandler<SetMainPhotoCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerSetMainPhotoCommand(
            IDataContext context,
            IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userAccessor = userAccessor;
            _userManager = userManager;
        }

        public async Task<Result<Unit>> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleAsync(x => x.UserName == _userAccessor.GetUsername());
            
            if(user == null)
                return null;
            
            var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

            if(photo == null)
                return null;

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null)
                currentMain.IsMain = false;
            
            photo.IsMain = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if(success)
                return Result<Unit>.Success(Unit.Value);

             return Result<Unit>.Failure("Problem setting main photo");
        }
    }
}