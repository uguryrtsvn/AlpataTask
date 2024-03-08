using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Constants;
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

        public MeetingService(IMeetingRepository meetingRepository, IMapper mapper) : base(meetingRepository, mapper)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task<IDataResult<MeetingDto>> GetMeetingWithId(Guid id)
        {
            var meet = _mapper.Map<MeetingDto>(await _meetingRepository.GetAsync(filter: x => x.Id == id, includes: z => z.Include(c => c.Inventories)));
            return meet != null ? new SuccessDataResult<MeetingDto>(meet, meet?.Name) : new ErrorDataResult<MeetingDto>(meet, "Toplantı bulunamadı.");
        }
        public async Task<IDataResult<List<MeetingDto>>> GetAllAsync()
        {
            var result = await _meetingRepository.GetFilteredList(selector: x => new Meeting
            {
                Id = x.Id,
                CreatorUserId = x.CreatorUserId,
                CreatedTime = x.CreatedTime,
                CreatorUser = x.CreatorUser,
                Description = x.Description,
                EndTime = x.EndTime,
                Inventories = x.Inventories,
                isActive = x.isActive,
                Name = x.Name,
                StartTime = x.StartTime,
            },
            expression: f => f.Id == Guid.Empty,
            includes: x => x.Include(c => c.CreatorUser)
            ); 

            result.ForEach(async x =>
            {
                if (x.StartTime < DateTime.Now)
                {
                    x.isActive = false;
                }
                await _meetingRepository.UpdateAsync(x);
            });
            var list = _mapper.Map<List<MeetingDto>>(result);
            return list != null ? new SuccessDataResult<List<MeetingDto>>(list, Messages.ListSuccess) : new ErrorDataResult<List<MeetingDto>>(list, Messages.ListFailed);
        }
    }
}
