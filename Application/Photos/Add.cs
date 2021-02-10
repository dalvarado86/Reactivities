using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Photos
{
    public class Add
    {
        public class CommandAddPhoto : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<CommandAddPhoto, Photo>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccesor;
            private readonly IPhotoAccessor _photoAccesor;

            public Handler(DataContext context, IUserAccessor userAccesor, IPhotoAccessor photoAccesor)
            {
                _photoAccesor = photoAccesor;
                _userAccesor = userAccesor;
                _context = context;
            }

            public async Task<Photo> Handle(CommandAddPhoto request, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoAccesor.AddPhoto(request.File);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccesor.GetCurrentUsername());

                var photo = new Photo 
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if(!user.Photos.Any(x => x.IsMain))
                    photo.IsMain = true;

                user.Photos.Add(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                    return photo;

                throw new Exception("Problem saving changes");
            }
        }
    }
}