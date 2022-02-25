
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Activities.Commands
{
    public class EditActivityCommand : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class EditActivityCommandValidator : AbstractValidator<EditActivityCommand>
    {
        public EditActivityCommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }

    public class HandlerEditActivityCommand : IRequestHandler<EditActivityCommand, Result<Unit>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public HandlerEditActivityCommand(IDataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);

            if(activity == null)
                return null;

           _mapper.Map(request.Activity, activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to update the activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}