using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonEndoVue.Server.Data;
using MonEndoVue.Server.Models;

namespace MonEndoVue.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DonneesDouleursController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DonneesDouleursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DonneesDouleurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonneesDouleur>>> GetDonneesDouleurs()
        {
            return await _context.DonneesDouleurs.ToListAsync();
        }

        // GET: DonneesDouleurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonneesDouleur>> GetDonneesDouleur(int id)
        {
            var donneesDouleur = await _context.DonneesDouleurs.FindAsync(id);

            if (donneesDouleur == null)
            {
                return NotFound();
            }

            return donneesDouleur;
        }

        // PUT: DonneesDouleurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonneesDouleur(int id, DonneesDouleur donneesDouleur)
        {
            if (id != donneesDouleur.Id)
            {
                return BadRequest();
            }

            _context.Entry(donneesDouleur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonneesDouleurExists(id))
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

        // POST: DonneesDouleurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DonneesDouleur>> PostDonneesDouleur(DonneesDouleur donneesDouleur)
        {
            _context.DonneesDouleurs.Add(donneesDouleur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonneesDouleur", new { id = donneesDouleur.Id }, donneesDouleur);
        }

        // DELETE: DonneesDouleurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonneesDouleur(int id)
        {
            var donneesDouleur = await _context.DonneesDouleurs.FindAsync(id);
            if (donneesDouleur == null)
            {
                return NotFound();
            }

            _context.DonneesDouleurs.Remove(donneesDouleur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonneesDouleurExists(int id)
        {
            return _context.DonneesDouleurs.Any(e => e.Id == id);
        }
    }
}
