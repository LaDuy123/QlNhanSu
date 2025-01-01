using BTL.Data;
using BTL.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongBanController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PhongBanController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/PhongBan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhongBanDto>>> GetPhongBan()
        {
            var phongBanList = await _context.PhongBans
                                             .Select(pb => new PhongBanDto
                                             {
                                                 MaPB = pb.MaPB,
                                                 TenPB = pb.TenPB
                                             })
                                             .ToListAsync();

            return Ok(phongBanList);
        }
    }
}
