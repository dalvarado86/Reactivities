using System.Threading.Tasks;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IPhotoAccessor
    {
         Task<PhotoUploadResult> AddPhoto(IFormFile file);
         Task<string> DeletePhoto(string publicId);
    }
}