using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecuenceBack.Models;

namespace SecuenceBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCenterController : ControllerBase
    {
        private readonly AppDBContext _context;

        public HealthCenterController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthCenterTbl>>> GetAll()
        {
            return Ok(await _context.HealthCenterTbl
                .AsNoTracking()
                .Where(healthCenter => healthCenter.DeletedAt == null)
                .OrderBy(healthCenter => healthCenter.HealthCenterID)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HealthCenterTbl>> GetById(int id)
        {
            var healthCenter = await _context.HealthCenterTbl
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.HealthCenterID == id && item.DeletedAt == null);

            if (healthCenter == null)
            {
                return NotFound(new { message = "Centro de salud no encontrado" });
            }

            return Ok(healthCenter);
        }

        [HttpPost]
        public async Task<ActionResult<HealthCenterTbl>> Create(HealthCenterTbl healthCenter)
        {
            healthCenter.HealthCenterID = 0;
            healthCenter.Status = healthCenter.Status == 0 ? 1 : healthCenter.Status;
            healthCenter.CreatedAt = DateTime.UtcNow;
            healthCenter.UpdatedAt = null;
            healthCenter.DeletedAt = null;

            _context.HealthCenterTbl.Add(healthCenter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = healthCenter.HealthCenterID }, healthCenter);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, HealthCenterTbl healthCenter)
        {
            var current = await _context.HealthCenterTbl
                .FirstOrDefaultAsync(item => item.HealthCenterID == id && item.DeletedAt == null);

            if (current == null)
            {
                return NotFound(new { message = "Centro de salud no encontrado" });
            }

            current.Name = healthCenter.Name;
            current.Country = healthCenter.Country;
            current.Direction = healthCenter.Direction;
            current.PhoneNumber = healthCenter.PhoneNumber;
            current.WebSite = healthCenter.WebSite;
            current.Status = healthCenter.Status;
            current.MedicCount = healthCenter.MedicCount;
            current.PatientsAvg = healthCenter.PatientsAvg;
            current.Response = healthCenter.Response;
            current.PoliciesAccepted = healthCenter.PoliciesAccepted;
            current.PoliciesAcceptedAt = healthCenter.PoliciesAcceptedAt;
            current.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var healthCenter = await _context.HealthCenterTbl
                .FirstOrDefaultAsync(item => item.HealthCenterID == id && item.DeletedAt == null);

            if (healthCenter == null)
            {
                return NotFound(new { message = "Centro de salud no encontrado" });
            }

            healthCenter.Status = 0;
            healthCenter.DeletedAt = DateTime.UtcNow;
            healthCenter.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}