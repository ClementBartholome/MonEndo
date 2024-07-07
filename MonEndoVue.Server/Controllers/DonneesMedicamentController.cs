using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonEndoVue.Server.Data;
using MonEndoVue.Server.Models;

namespace MonEndoVue.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DonneesMedicamentController(AppDbContext context) : ControllerBase
    {
        // GET: DonneesMedicament
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonneesMedicament>>> GetDonneesMedicaments()
        {
            return await context.DonneesMedicaments.ToListAsync();
        }
        
        // GET: DonneesMedicament/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonneesMedicament>> GetDonneesMedicament(int id)
        {
            var donneesMedicament = await context.DonneesMedicaments.FindAsync(id);

            if (donneesMedicament == null)
            {
                return NotFound();
            }

            return donneesMedicament;
        }
        
        // PUT: DonneesMedicament/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonneesMedicament(int id, DonneesMedicament donneesMedicament)
        {
            if (id != donneesMedicament.Id)
            {
                return BadRequest();
            }

            context.Entry(donneesMedicament).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonneesMedicamentExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }
        
        // POST: DonneesMedicament
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DonneesMedicament>> PostDonneesMedicament(DonneesMedicament donneesMedicament)
        {
            context.DonneesMedicaments.Add(donneesMedicament);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetDonneesMedicament", new { id = donneesMedicament.Id }, donneesMedicament);
        }

        private bool DonneesMedicamentExists(int id)
        {
            return context.DonneesMedicaments.Any(e => e.Id == id);
        }
        
        // DELETE: DonneesMedicament/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonneesMedicament(int id)
        {
            var donneesMedicament = await context.DonneesMedicaments.FindAsync(id);
            if (donneesMedicament == null)
            {
                return NotFound();
            }

            context.DonneesMedicaments.Remove(donneesMedicament);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}