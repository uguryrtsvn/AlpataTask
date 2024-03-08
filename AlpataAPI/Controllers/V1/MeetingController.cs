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
        public MeetingController(IMeetingService meetingService, IMapper mapper)
        {
            _meetingService = meetingService; 
        }
        [HttpGet]
        public async Task<IActionResult> GetMeetingWithId(string Id)
        {
            var result = await _meetingService.GetMeetingWithId(Guid.Parse(Id));
            return result.Success ? Ok(result) : BadRequest(result);
        }    
        [HttpGet]
        public async Task<IActionResult> AddUserToMeeting(string meetId)
        {
            var result = await _meetingService.AddUserToMeeting(Guid.Parse(meetId),parsedUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }     
        [HttpGet]
        public async Task<IActionResult> DeleteUserToMeeting(string meetId)
        {
            var result = await _meetingService.DeleteUserToMeeting(Guid.Parse(meetId),parsedUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _meetingService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingDto meeting)
        {
            meeting.CreatorUserId = Guid.Parse(userId);
            var result = await _meetingService.CreateAsync(meeting);
            return result.Success ? Ok(result) : BadRequest(result);
        } 
    }
}
