using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Comments.Queries
{
    public class GetCommentsQuery : IRequest<Result<List<CommentDto>>>
    {
        public Guid ActivityId { get; set; }
    }

    public class HandlerGetCommentsCommand : IRequestHandler<GetCommentsQuery, Result<List<CommentDto>>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public HandlerGetCommentsCommand(IDataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<CommentDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Where(x => x.Activity.Id == request.ActivityId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Result<List<CommentDto>>.Success(comments);
        }
    }
}