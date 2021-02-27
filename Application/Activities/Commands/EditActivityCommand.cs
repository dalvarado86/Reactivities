using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Activities.Commands
{
    public class EditActivityCommand : IRequest
    {
        public Activity Activity { get; set; }
    }

    public class HandlerEditActivityCommand : IRequestHandler<EditActivityCommand>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public HandlerEditActivityCommand(IDataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);

           _mapper.Map(request.Activity, activity);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
                return Unit.Value;

            throw new Exception("Problem saving changes");
        }
    }
}