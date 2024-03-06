using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.Base;
using AlpataDAL.IRepositories;
using AlpataEntities.Dtos.MeetingDtos;
using AlpataEntities.Entities.Concretes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Concretes
{
    public class MeetingService : BaseService<Meeting>, IMeetingService
    {
        readonly IMeetingRepository _meetingRepository;

        public MeetingService(IMeetingRepository meetingRepository, IMapper mapper) : base(meetingRepository,mapper)
        {
            _meetingRepository = meetingRepository;
        }
         
        public async Task<IDataResult<MeetingDto>> GetMeetingWithId(Guid id)
        {
           var meet = _mapper.Map<MeetingDto>(await _meetingRepository.GetAsync(filter: x => x.Id == id, includes: z => z.Include(c => c.Inventories)));
            return meet !=null ? new SuccessDataResult<MeetingDto>(meet, meet?.Name) : new ErrorDataResult<MeetingDto>(meet,"Toplantı bulunamadı.");
        }
    }
}
