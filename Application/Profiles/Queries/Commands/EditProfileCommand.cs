using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Profiles.Queries.Commands
{
    public class EditProfileCommand : IRequest<Result<Unit>>
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }

    public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileCommandValidator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty();
        }
    }

    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditProfileCommandHandler(
            IDataContext context,
            IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _userAccessor = userAccessor;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<Unit>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.GetUsername());

            user.Bio = request.Bio ?? user.Bio;
            user.DisplayName = request.DisplayName ?? user.DisplayName;

            var success = await _userManager.UpdateAsync(user);

            if (success.Succeeded)
            {
                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure("Problem updating profile");
        }
    }
}