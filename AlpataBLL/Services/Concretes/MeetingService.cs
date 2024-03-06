using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.BaseService;
using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Concretes;
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

        public MeetingService(IMeetingRepository meetingRepository) : base(meetingRepository)
        {
            _meetingRepository = meetingRepository;
        } 
    }
}
