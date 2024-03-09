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
        
        public async Task<IDataResult<List<MeetingDto>>> GetOrganizedMeetings(Guid id)
        {
            var meet = _mapper.Map<List<MeetingDto>>(await _meetingRepository.GetAllAsync(filter: x => x.CreatorUserId == id, includes: z => z.Include(a => a.Participants).Include(x => x.CreatorUser)));
            return meet != null ? new SuccessDataResult<List<MeetingDto>>(meet,Messages.ListSuccess) : new ErrorDataResult<List<MeetingDto>>(meet, Messages.ListSuccess);
        }
        public async Task<IDataResult<List<MeetingDto>>> GetAllAsync()
        {
         
            var list = await _meetingRepository.GetAllAsync(z => z.Id != Guid.Empty, includes: i => i.Include(m => m.Participants).Include(z => z.CreatorUser));
            //Burada başlangıç süreleri geçmiş toplantılar için bir Background servis veya başka şekillerle daha anlamlı bir çözüm getirilebilir ama 
            //şimdilik böyle ameleus bir yöntem kullandım.
            foreach (var x in list)
            {
                if (x.StartTime < DateTime.Now && x.isActive)
                {
                    x.isActive = false;
                    await _meetingRepository.UpdateAsync(x);
                }
            }
            var result = _mapper.Map<List<MeetingDto>>(list.OrderByDescending(z => z.CreatedTime)); 
            return result != null ? new SuccessDataResult<List<MeetingDto>>(result, Messages.ListSuccess) : new ErrorDataResult<List<MeetingDto>>(result, Messages.ListFailed);
        }

        public async Task<IDataResult<bool>> AddUserToMeeting(Guid meetId, Guid userId)
        {
            var user = await _userService.GetAsync(z => z.Id == userId); 
            var m = await _meetingRepository.GetAsync(z => z.Id == meetId);
            
            return await RunTransaction<bool>(async () =>
            {
            var mp = await _participantRepository.CreateAsync(new()
            { 
                AppUserId = userId, 
                MeetingId = meetId
            });
            if (mp)
            {
                    await _emailService.SendEmail(new()
                    {
                        Subject = "Toplantı Bilgileri",
                        Content = GenerateMeetingHtml(m),
                        Receiver = user?.Email
                    });
                    return new SuccessDataResult<bool>(mp);
            }
            return new ErrorDataResult<bool>(mp);
            });
        }      
        public async Task<IDataResult<bool>> DeleteUserFromMeeting(Guid meetId, Guid userId)
        { 
           var result = await _participantRepository.DeleteAsync(await _participantRepository.GetAsync(z => z.MeetingId == meetId && z.AppUserId == userId));
           return result ? new SuccessDataResult<bool>(result) : new ErrorDataResult<bool>(result);
        }




        public string GenerateMeetingHtml(Meeting m)
        {
            string htmlContent = $@"<!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>{m?.Name} - Toplantı Bilgileri</title>
        <style>
            table {{
                border-collapse: collapse;
                width: 100%;
            }}
            th, td {{
                border: 1px solid #dddddd;
                text-align: left;
                padding: 8px;
            }}
            th {{
                background-color: #f2f2f2;
            }}
            img {{
                display: block;
                margin: 0 auto;
                max-width: 200px;
                max-height: 200px;
                background-color: #333;
                padding: 10px;
            }}
        </style>
    </head>
    <body>
        <img src=""https://alpatateknoloji.com/images/logo1.png"" alt=""Company Logo"">
        <h2>{m?.Name} - Toplantı Bilgileri</h2>
        <table>
            <tr>
                <th>Başlık</th>
                <th>İçerik</th>
            </tr>
            <tr><td>Başlangıç Zamanı</td><td>{m?.StartTime}</td></tr>
            <tr><td>Bitiş Tarihi</td><td>{m?.EndTime}</td></tr>
            <tr><td>Katılımcı sayısı:</td><td>{m?.Participants.Count}</td></tr>
        </table>
    </body>
    </html>";

            return htmlContent;
        }
    }
}
