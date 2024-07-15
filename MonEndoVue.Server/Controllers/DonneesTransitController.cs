using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonEndoVue.Server.Data;
using MonEndoVue.Server.Models;

namespace MonEndoVue.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DonneesTransitController(AppDbContext context) : ControllerBase
    {
        // GET: DonneesTransit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonneesTransit>>> GetDonneesTransit()
        {
            return await context.DonneesTransit.ToListAsync();
        }
        
        // GET: DonneesTransit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonneesTransit>> GetDonneesTransit(int id)
        {
            var donneesTransit = await context.DonneesTransit.FindAsync(id);

            if (donneesTransit == null)
            {
                return NotFound();
            }

            return donneesTransit;
        }
        
        // POST: DonneesTransit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DonneesTransit>> PostDonneesTransit(DonneesTransit donneesTransit)
        {
            context.DonneesTransit.Add(donneesTransit);
            await context.SaveChangesAsync();


            return CreatedAtAction("GetDonneesTransit", new { id = donneesTransit.Id }, donneesTransit);
        }
        
        // PUT: DonneesTransit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonneesTransit(int id, DonneesTransit donneesTransit)
        {
            if (id != donneesTransit.Id)
            {
                return BadRequest();
            }

            context.Entry(donneesTransit).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonneesTransitExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }
        
        private bool DonneesTransitExists(int id)
        {
            return context.DonneesTransit.Any(e => e.Id == id);
        }
        
        // DELETE: DonneesTransit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonneesTransit(int id)
        {
            var donneesTransit = await context.DonneesTransit.FindAsync(id);
            if (donneesTransit == null)
            {
                return NotFound();
            }

            context.DonneesTransit.Remove(donneesTransit);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}