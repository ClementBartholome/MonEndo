using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonEndoVue.Server.Data;
using MonEndoVue.Server.Models;

namespace MonEndoVue.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DonneesActivitePhysiqueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DonneesActivitePhysiqueController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DonneesActivitePhysique
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonneesActivitePhysique>>> GetDonneesActivitePhysique()
        {
            return await _context.DonneesActivitePhysique.ToListAsync();
        }

        // GET: api/DonneesActivitePhysique/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonneesActivitePhysique>> GetDonneesActivitePhysique(int id)
        {
            var donneesActivitePhysique = await _context.DonneesActivitePhysique.FindAsync(id);

            if (donneesActivitePhysique == null)
            {
                return NotFound();
            }

            return donneesActivitePhysique;
        }

        // PUT: api/DonneesActivitePhysique/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonneesActivitePhysique(int id, DonneesActivitePhysique donneesActivitePhysique)
        {
            if (id != donneesActivitePhysique.Id)
            {
                return BadRequest();
            }

            _context.Entry(donneesActivitePhysique).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonneesActivitePhysiqueExists(id))
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

        // POST: api/DonneesActivitePhysique
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DonneesActivitePhysique>> PostDonneesActivitePhysique(DonneesActivitePhysique donneesActivitePhysique)
        {
            _context.DonneesActivitePhysique.Add(donneesActivitePhysique);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonneesActivitePhysique", new { id = donneesActivitePhysique.Id }, donneesActivitePhysique);
        }

        // DELETE: DonneesActivitePhysique/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonneesActivitePhysique(int id)
        {
            var donneesActivitePhysique = await _context.DonneesActivitePhysique.FindAsync(id);
            if (donneesActivitePhysique == null)
            {
                return NotFound();
            }

            _context.DonneesActivitePhysique.Remove(donneesActivitePhysique);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonneesActivitePhysiqueExists(int id)
        {
            return _context.DonneesActivitePhysique.Any(e => e.Id == id);
        }
    }
}
