using AlpataBLL.Services.Abstracts;
using AlpataEntities.Dtos.MeetingDtos; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers.V2
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")] 
    [ApiVersion("2.0")]
    public class Meeting2Controller : ControllerBase
    {
        readonly IMeetingService _meetingService;
        public Meeting2Controller(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMeetingWithId(string id)
        {
          var result =  await _meetingService.GetMeetingWithId(Guid.Parse(id));
         return result.Success ? Ok(result) : BadRequest(result);
        }
        [MapToApiVersion("2.0")]
        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingDto meeting)
        {
            var result = await _meetingService.CreateAsync(meeting);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
