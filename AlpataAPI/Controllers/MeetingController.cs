using AlpataBLL.Services.Abstracts;
using AlpataEntities.Dtos.MeetingDtos;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlpataAPI.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeetingController : BaseController
    {
        readonly IMeetingService _meetingService;
        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMeetingWithId(string id)
        {
          var result =  await _meetingService.GetMeetingWithId(Guid.Parse(id));
         return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingDto meeting)
        {
            var result = await _meetingService.CreateAsync(meeting);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
