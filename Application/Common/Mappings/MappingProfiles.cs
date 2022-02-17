using System.Linq;
using Application.Comments.DTOs;
using Application.Common.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUserName, 
                    o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost).ApplicationUser.UserName));

            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.ApplicationUser.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.ApplicationUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.ApplicationUser.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.ApplicationUser.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<ApplicationUser, Models.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}