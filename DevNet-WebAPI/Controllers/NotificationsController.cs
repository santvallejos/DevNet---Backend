using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Models;
using DevNet_WebAPI.Infrastructure.DTO;

namespace DevNet_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly DevnetDBContext _context;

        public NotificationsController(DevnetDBContext context)
        {
            _context = context;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return notification;
        }

        // PUT: api/Notifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*
         * Codigo comentado en caso de que se quiera poder editar las notificaciones
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(Guid id, Notification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: api/Notifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Notification>> SendNotification(SendNotificationDto notificationDto)
        {
            Notification notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = notificationDto.UserId,
                Type = notificationDto.Type,
                Text = notificationDto.Text,
                RelatedUserId = notificationDto.UserId,
                PostId = notificationDto.PostId,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotification", new { id = notification.Id }, notification);
        }

        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(Guid id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
