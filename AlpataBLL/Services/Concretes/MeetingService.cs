using AlpataBLL.BaseResult.Abstracts;
using AlpataBLL.BaseResult.Concretes;
using AlpataBLL.Constants;
using AlpataBLL.Services.Abstracts;
using AlpataBLL.Services.Base;
using AlpataBLL.Services.EmailService;
using AlpataDAL.IRepositories;
using AlpataEntities.Dtos.AuthDtos;
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
        readonly IAppUserRepository _userService;
        private readonly IEmailService _emailService;
        private readonly IMeetingParticipantRepository _participantRepository;

        public MeetingService(IMeetingRepository meetingRepository, IMapper mapper, IAppUserRepository userService, IEmailService emailService, IMeetingParticipantRepository participantRepository) : base(meetingRepository, mapper)
        {
            _meetingRepository = meetingRepository;
            _userService = userService;
            _emailService = emailService;
            _participantRepository = participantRepository;
        }

        public async Task<IDataResult<MeetingDto>> GetMeetingWithId(Guid id)
        {
            var meet = _mapper.Map<MeetingDto>(await _meetingRepository.GetAsync(filter: x => x.Id == id, includes: z => z.Include(c => c.Inventories).Include(a => a.Participants).Include(x => x.CreatorUser)));
            return meet != null ? new SuccessDataResult<MeetingDto>(meet, meet?.Name) : new ErrorDataResult<MeetingDto>(meet, "Toplantı bulunamadı.");
        }
        public async Task<IDataResult<List<MeetingDto>>> GetAllAsync()
        {
         
            var list = await _meetingRepository.GetAllAsync(z => z.Id != Guid.Empty, includes: i => i.Include(m => m.Participants).Include(z => z.CreatorUser));
            foreach (var x in list)
            {
                if (x.StartTime < DateTime.Now && x.isActive)
                {
                    x.isActive = false;
                    await _meetingRepository.UpdateAsync(x);
                }
            }
            var result = _mapper.Map<List<MeetingDto>>(list);
            return list != null ? new SuccessDataResult<List<MeetingDto>>(result, Messages.ListSuccess) : new ErrorDataResult<List<MeetingDto>>(result, Messages.ListFailed);
        }

        public async Task<IDataResult<bool>> AddUserToMeeting(Guid meetId, Guid userId)
        {
            //var user = await _userService.GetAsync(z => z.Id == userId);
            var meet = await _meetingRepository.GetAsync(filter: x => x.Id == meetId, includes: z => z.Include(c => c.Inventories).Include(a => a.Participants).Include(x => x.CreatorUser));

            var m = await _meetingRepository.GetAsync(z => z.CreatorUserId == userId, includes: x => x.Include(z => z.Participants));

            return await RunTransaction<bool>(async () =>
            {
            var mp = await _participantRepository.CreateAsync(new()
            {
                //AppUser = user,
                AppUserId = userId,
                //Meeting = meet,
                MeetingId = meetId
            });
            if (mp)
            {
                //await _emailService.SendEmail(new()
                //{
                //    Subject = "Email Adresinizi Onaylamalısınız.",
                //    Content = CreateHtmlBody("ApproveConfirm", new Dictionary<string, string>
                //    {
                //        { "{username}", string.Join(' ', entity.Name, entity.Surname) },
                //        { "{applicationLink}", $"{AppSettingsHelper.PresentationUrl}Account/ConfirmedEmail?tkn={uv.Token}" }
                //    }),
                //    Receiver = user.Email
                //});
                return new SuccessDataResult<bool>(mp);
            }
            return new ErrorDataResult<bool>(mp);
            });
        }
    }
}
