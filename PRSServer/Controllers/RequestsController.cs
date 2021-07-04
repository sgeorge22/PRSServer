using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSServer.Data;
using PRSServer.Models;

namespace PRSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PRSServerContext _context;

        public RequestsController(PRSServerContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }
            return request;
        }

        //GET: api/Request/detailed
        [HttpGet("detailed")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsDetailed()
        {
            return await _context.Requests
                .Include(u => u.User)
                .Include(r => r.RequestLines)
                .ThenInclude(p => p.Product)
                .ToListAsync();
        }

        //GET: api/Requests/5/detailed
        [HttpGet("{id}/detailed")]
        public async Task<ActionResult<Request>> GetRequestDetailed(int id)
        {
            var request = await _context.Requests
                .Include(u => u.User)
                .Include(r => r.RequestLines)
                .ThenInclude(p => p.Product)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }
            return request;
        }

        //GET: api/Requests/id/user
        [HttpGet("{id}/user")]
        public async Task<ActionResult<Request>> GetRequestAndUser(int id)
        {
            var request = await _context.Requests
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id);
            if(request == null)
            {
                return NotFound();
            }
            return request;
        }

        //GET: api/Requests/underReview/5
        [HttpGet("underReview/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestUnderReview(int id)
        {
            return await _context.Requests
                .Include(u => u.User)
                .Include(r => r.RequestLines)
                .ThenInclude(p => p.Product)
                .Where(r => r.Status == "REVIEW" && r.UserId != id)
                .ToListAsync();
        }

        // PUT: api/Requests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        //PUT: api/Requests/id/review
        [HttpPut("{id}/review")]
        public async Task<IActionResult> PutToReview(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            var total = request.Total;

            if(request == null)
            {
                return NotFound();
            }
            request.Status = (request.Total <= 50 && request.Total > 0) ? "APPROVED" : "REVIEW";
            return await PutRequest(id, request);
        }

        //PUT: api/Request/id/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> PutToApprove(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            var total = request.Total;

            if(request == null)
            {
                return NotFound();
            }
            request.Status = "APPROVED";
            return await PutRequest(id, request);
        }

        //PUT: api/Requests/id/reject
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> PutToReject(int id, Request request)
        {
            request.Status = "REJECTED";

            return await PutRequest(id, request);
            
            
        }

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
