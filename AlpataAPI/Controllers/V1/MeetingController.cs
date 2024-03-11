using AlpataBLL.Services.Abstracts;
using AlpataEntities.Dtos.MeetingDtos;
using AlpataEntities.Entities.Concretes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers.V1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")] 
    [ApiVersion("1.0")]
    public class MeetingController : BaseController
    {
        readonly IMeetingService _meetingService; 
        readonly IMapper _mapper; 
        public MeetingController(IMeetingService meetingService, IMapper mapper)
        {
            _meetingService = meetingService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetMeetingWithId(string meetId)
        {
            var result = await _meetingService.GetMeetingWithId(Guid.Parse(meetId));
            return result.Success ? Ok(result) : BadRequest(result);
        }     
        [HttpGet]
        public async Task<IActionResult> GetOrganizedMeetings(string Id)
        {
            var result = await _meetingService.GetOrganizedMeetings(Guid.Parse(userId));
            return result.Success ? Ok(result) : BadRequest(result);
        }    
        [HttpGet]
        public async Task<IActionResult> AddUserToMeeting(string meetId)
        {
            var result = await _meetingService.AddUserToMeeting(Guid.Parse(meetId),parsedUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }     
        [HttpGet]
        public async Task<IActionResult> DeleteUserFromMeeting(string meetId)
        {
            var result = await _meetingService.DeleteUserFromMeeting(Guid.Parse(meetId),parsedUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _meetingService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }   
        [HttpGet]
        public async Task<IActionResult> GetFilteredMeetings(string q)
        {
            var result = await _meetingService.GetAllAsync<MeetingDto>(z=>z.Name.Contains(q)); 
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingDto dto)
        {
            dto.CreatorUserId = Guid.Parse(userId);
            var result = await _meetingService.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }    
        [HttpPost]
        public async Task<IActionResult> EditMeeting(MeetingDto dto)
        {
            var meet =await _meetingService.GetAsync(z => z.Id == dto.Id);
            meet.Data.StartTime = dto.StartTime;
            meet.Data.EndTime = dto.EndTime;
            meet.Data.Name =dto.Name;
            meet.Data.Description = dto.Description;
            var result =await  _meetingService.UpdateAsync(meet.Data);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMeeting(string Id)
        {
            var result = await _meetingService.DeleteTrunsactionAsync(Guid.Parse(Id));
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
