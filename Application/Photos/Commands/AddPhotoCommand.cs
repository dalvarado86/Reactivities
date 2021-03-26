using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Photos.Commands
{
    public class AddPhotoCommand : IRequest<Result<Photo>>
    {
        public IFormFile File { get; set; }
    }

    public class HandlerAddPhotoCommand : IRequestHandler<AddPhotoCommand, Result<Photo>>
    {
        private readonly IDataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerAddPhotoCommand(
            IDataContext context, 
            IPhotoAccessor photoAccessor,
            IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _photoAccessor = photoAccessor;
            _context = context;
            _userAccessor = userAccessor;
            _userManager = userManager;
        }

        public async Task<Result<Photo>> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleAsync(x => x.UserName == _userAccessor.GetUsername());
        
            if(user == null)
                return null;

            var photoUploadResult = await _photoAccessor.AddPhoto(request.File);

            var photo = new Photo 
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId
            };

            if(!user.Photos.Any(x => x.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if(result)
                return Result<Photo>.Success(photo);
            
            return Result<Photo>.Failure("Problem adding photo");
        }
    }
}