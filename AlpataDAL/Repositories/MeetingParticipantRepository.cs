using AlpataDAL.IRepositories;
using AlpataEntities.Entities.Concretes;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlpataDAL.Repositories
{
    public class MeetingParticipantRepository : BaseRepository<MeetingParticipant>, IMeetingParticipantRepository
    {
        public MeetingParticipantRepository(AlpataDbContext db, IMapper mapper) : base(db, mapper)
        {
        }
    }
}
