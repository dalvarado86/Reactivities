using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Activities.Commands
{
    public class CreateActivityCommand : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class HandlerCreateActivityCommand : IRequestHandler<CreateActivityCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerCreateActivityCommand(
            IDataContext context,
            IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.GetUsername());

            var attendee = new ActivityAttendee
            {
                ApplicationUser = user,
                Activity = request.Activity,
                IsHost = true
            };

            request.Activity.Attendees.Add(attendee);

            _context.Activities.Add(request.Activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to create activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }

}