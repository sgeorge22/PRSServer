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
    public class RequestLinesController : ControllerBase
    {
        private readonly PRSServerContext _context;

        
        private async Task RecalculateRequestTotal(int reqid)
        {
            var request = await _context.Requests.FindAsync(reqid);
            if (request == null) throw new Exception("FATAL: Request is not found to recalculate!");
            request.Total = (from rl in _context.RequestLines
                             join p in _context.Products
                             on rl.ProductId equals p.Id
                             where rl.RequestId == reqid
                             select new { LineTotal = rl.Quantity * p.Price }).Sum(x => x.LineTotal);
            await _context.SaveChangesAsync();
        }

        public RequestLinesController(PRSServerContext context)
        {
            _context = context;
        }

        // GET: api/RequestLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLines()
        {
            return await _context.RequestLines.ToListAsync();
        }

        //GET: api/RequestLines/detailed
        [HttpGet("detailed")]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLinesDetailed()
        {
            return await _context.RequestLines
                .Include(r => r.Request).ThenInclude(u => u.User)
                .Include(p => p.Product).ThenInclude(v => v.Vendor)
                .ToListAsync();
        }

        // GET: api/RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);

            if (requestLine == null)
            {
                return NotFound();
            }

            return requestLine;
        }

        //GET: api/RequestLines/5/detailed
        [HttpGet("{id}/detailed")]
        public async Task<ActionResult<RequestLine>> GetRequestLineDetailed(int id)
        {
            var requestLine = await _context.RequestLines
                .Include(r => r.Request).ThenInclude(u => u.User)
                .Include(p => p.Product).ThenInclude(v => v.Vendor)
                .SingleOrDefaultAsync(rl => rl.Id == id);

            if(requestLine == null)
            {
                return NotFound();
            }
            return requestLine;
        }

        // PUT: api/RequestLines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(id))
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

        // POST: api/RequestLines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        // DELETE: api/RequestLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RequestLine>> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();

            return requestLine;
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }
    }
}
