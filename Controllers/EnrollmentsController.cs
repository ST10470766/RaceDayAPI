using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrollmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> Enroll(EnrollmentDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Check if already enrolled
            var existing = _context.EventEnrollments.FirstOrDefault(e => e.ParticipantId == userId && e.EventId == request.EventId);
            if (existing != null) return BadRequest("Already enrolled in this event.");

            var enrollment = new EventEnrollment
            {
                ParticipantId = userId,
                EventId = request.EventId,
                CategoryId = request.CategoryId
            };

            _context.EventEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Successfully enrolled." });
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Organiser")]
        public IActionResult GetEventEnrollments(int eventId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Verify Organiser owns the event
            var eventObj = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (eventObj == null) return NotFound();
            if (eventObj.OrganiserId != userId) return Forbid("You do not own this event.");

            var enrollments = _context.EventEnrollments
                .Where(e => e.EventId == eventId)
                .Select(e => new
                {
                    e.Id,
                    ParticipantName = e.Participant.Name + " " + e.Participant.Surname,
                    CategoryName = e.Category.Name
                }).ToList();

            return Ok(enrollments);
        }
    }
}
