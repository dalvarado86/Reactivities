using System.Threading.Tasks;
using API.DTOs;
using Application.Profiles.Queries;
using Application.Profiles.Queries.Commands;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new GetProfileQuery { Username = username }));
        }

         [HttpPut]
        public async Task<IActionResult> Edit(ProfileRequestDto profile)
        {
            return HandleResult(await Mediator.Send(new EditProfileCommand { DisplayName = profile.DisplayName, Bio = profile.Bio }));
        }


        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new GetActivitiesQuery { Username = username, Predicate = predicate }));
        }
    }
}