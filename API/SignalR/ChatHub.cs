using System;
using System.Threading.Tasks;
using Application.Comments.Commands;
using Application.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
         
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(CreateCommentCommand command)
        {            
            var comment = await _mediator.Send(command);
                
            await Clients.Group(command.ActivityId.ToString())
                    .SendAsync("ReceiveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            var result = await _mediator.Send(new GetCommentsQuery { ActivityId = activityId });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}