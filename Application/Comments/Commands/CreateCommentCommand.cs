using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Comments.Commands
{
    public class CreateCommentCommand : IRequest<Result<CommentDto>>
    {
        public string Body { get; set; }
        public Guid ActivityId { get; set; }
    }

    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();            
        }
    }

    public class HandlerCreateCommentCommand : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HandlerCreateCommentCommand(IDataContext context, IMapper mapper, IUserAccessor userAccessor, UserManager<ApplicationUser> userManager)
        {
            _userAccessor = userAccessor;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.ActivityId);

            if (activity is null)
            {
                return null;
            }

            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            var comment = new Comment
            {
                Author = user,
                Activity = activity,
                Body = request.Body,
            };

            activity.Comments.Add(comment);

            var succes = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (succes)
            {
                return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
            }

            return Result<CommentDto>.Failure("Failure to add comment");
        }
    }
}