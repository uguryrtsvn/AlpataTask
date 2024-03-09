using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.Services.Base;
using AlpataEntities.Dtos.MeetingDtos;
using AlpataEntities.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataBLL.Services.Abstracts
{
    public interface IMeetingService : IBaseService<Meeting>
    {
        Task<IDataResult<MeetingDto?>> GetMeetingWithId(Guid id);
        Task<IDataResult<List<MeetingDto>>> GetAllAsync();
        Task<IDataResult<List<MeetingDto>>> GetOrganizedMeetings(Guid userId);
        Task<IDataResult<bool>> AddUserToMeeting(Guid meetId, Guid userId);
        Task<IDataResult<bool>> DeleteUserFromMeeting(Guid meetId, Guid userId);
    }
}
