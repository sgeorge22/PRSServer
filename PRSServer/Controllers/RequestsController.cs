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

        //review request 
        [HttpPut("{id}/review")]
        public async Task<IActionResult> ReviewOrApproveRequest(int id)
        {
            var req = await _context.Requests.FindAsync(id);
            if (req == null) { return NotFound(); }
            if (req.Total == 0) { throw new Exception("Request not valid - total equals $0"); }
            req.Status = (req.Total <= 50 && req.Total > 0) ? "APPROVED" : "REVIEW";

            return await PutRequest(id, req);  
        }
        //sets status to approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> PutRequestStatusToApprove(int id)
        {
            var req = await _context.Requests.FindAsync(id);
            if (req == null) { return NotFound(); }
            req.Status = "APPROVED";
            return await PutRequest(id, req);
        }
        //sets status to reject
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> PutRequestStatusToReject(int id)
        {
            var req = await _context.Requests.FindAsync(id);
            if (req == null) { return NotFound(); }
            req.Status = "REJECTED";
            return await PutRequest(id, req);
        }
        ///return requests where status is in review and not owned by user on request
        [HttpGet("{userid}/reviews")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsByReviewStatus(int userid)
        {
            return await _context.Requests
                                 .Where(x => x.Status == "REVIEW" && x.UserId != userid)
                                 .ToListAsync();
        }
        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests
                .Include(r => r.User) //add to access user name when viewing products in web app
                .ToListAsync();
        }
        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests
                .Include(r => r.User)
                .Include(r => r.RequestLines)
                .ThenInclude(rl => rl.Product)
                .SingleOrDefaultAsync(r => r.Id == id);  //replaced find which can only be used when id is the only primary key

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }
        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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

        // POST: api/Requests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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
