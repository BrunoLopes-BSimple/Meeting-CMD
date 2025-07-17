using Application.DTO;
using Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers
{
    [ApiController]
    [Route("api/meeting")]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpPost]
        public async Task<ActionResult<CreatedMeetingDTO>> Create([FromBody] CreateMeetingDTO dto)
        {
            var createdMeeting = await _meetingService.Create(dto);
            return createdMeeting.ToActionResult();
        }

        [HttpPut]
        public async Task<ActionResult<EditedMeetingDTO>> Edit([FromBody] EditMeetingDTO dto)
        {
            var editedMeeting = await _meetingService.EditMeeting(dto);
            return editedMeeting.ToActionResult();
        }
    }
}