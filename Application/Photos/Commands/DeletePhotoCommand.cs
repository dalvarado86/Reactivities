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
    public class DeletePhotoCommand : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }

    public class HandlerDeletePhotoCommand : IRequestHandler<DeletePhotoCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerDeletePhotoCommand(
            IDataContext context, 
            IPhotoAccessor photoAccessor,
            IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
            _userManager = userManager;
        }

        public async Task<Result<Unit>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
             var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleAsync(x => x.UserName == _userAccessor.GetUsername());

            if(user == null)
                return null;
            
            var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

            if(photo == null)
                return null;
            
            if(photo.IsMain)
                return Result<Unit>.Failure("You cannot delete your main photo");
            
            var result = await _photoAccessor.DeletePhoto(photo.Id);

            if(result == null)
                return Result<Unit>.Failure("Problem deleting photo from cloudinary");

            user.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if(success)
                return Result<Unit>.Success(Unit.Value);
            
            return Result<Unit>.Failure("Problem deleting photo from cloudinary");
        }
    }
}