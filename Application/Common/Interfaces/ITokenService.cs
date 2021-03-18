using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(ApplicationUser user);
    }
}