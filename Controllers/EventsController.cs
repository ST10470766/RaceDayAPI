using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.Include(e => e.Categories).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.Include(e => e.Categories).FirstOrDefaultAsync(e => e.Id == id);
            if (@event == null) return NotFound();
            return @event;
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<ActionResult<Event>> CreateEvent(EventDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var newEvent = new Event
            {
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location,
                Distance = request.Distance,
                EventType = request.EventType,
                OrganiserId = userId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null) return NotFound();
            if (existingEvent.OrganiserId != userId) return Forbid("You can only edit your own events.");

            existingEvent.Name = request.Name;
            existingEvent.Description = request.Description;
            existingEvent.Date = request.Date;
            existingEvent.Location = request.Location;
            existingEvent.Distance = request.Distance;
            existingEvent.EventType = request.EventType;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null) return NotFound();
            if (existingEvent.OrganiserId != userId) return Forbid("You can only delete your own events.");

            _context.Events.Remove(existingEvent);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
