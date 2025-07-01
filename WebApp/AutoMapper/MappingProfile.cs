using AutoMapper;
using eventLib.Models;
using WebApp.ViewModels;

namespace exercise_13.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserVM>();
            CreateMap<EventType, EventTypeVM>();
            CreateMap<Event, EventVM>();
            CreateMap<EventPerformer, PerformerVM>();

            // CreateMap<Audio, SongVM>()
            //      .ForMember(dst => dst.TagIds, opt => opt.MapFrom(src => src.AudioTags.Select(x => x.TagId)));
        }
    }
}
