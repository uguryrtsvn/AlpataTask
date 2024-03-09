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
            CreateMap<Meeting, MeetingDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember, context) =>
                                                         {
                                                             if (srcMember is Guid guidValue)
                                                             {
                                                                 return guidValue != Guid.Empty;
                                                             }
                                                             return srcMember != null;
                                                         }));
        }
    }
}
