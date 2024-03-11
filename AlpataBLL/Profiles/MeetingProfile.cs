using AlpataEntities.Dtos.MeetingDtos;
using AlpataEntities.Entities.Concretes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Profiles
{
    public class MeetingProfile : Profile
    {
        public MeetingProfile()
        {
            CreateMap<Meeting, MeetingDto>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.CreatorUserId, opt => opt.MapFrom(src => src.CreatorUserId))
      .ForMember(dest => dest.CreatorUser, opt => opt.MapFrom(src => src.CreatorUser))
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
      .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
      .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
      .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
      .ForMember(dest => dest.Inventories, opt => opt.MapFrom(src => src.Inventories))
      .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
      .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime));

            CreateMap<MeetingDto, Meeting>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatorUserId, opt => opt.MapFrom(src => src.CreatorUserId))
                .ForMember(dest => dest.CreatorUser, opt => opt.MapFrom(src => src.CreatorUser))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Inventories, opt => opt.MapFrom(src => src.Inventories))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
                .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime));
        }
    }
}
