using BTL.Data;
using BTL.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HocVanController : ControllerBase
    {
        private readonly MyDbContext _context;

        public HocVanController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/TrinhDoHocVan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrinhDoHocVanDto>>> GetTrinhDoHocVan()
        {
            var trinhDoHocVanList = await _context.TrinhDoHocVans
                                                   .Select(td => new TrinhDoHocVanDto
                                                   {
                                                       MaTDHV = td.MaTDHV,
                                                       TenTDHV = td.TenTDHV
                                                   })
                                                   .ToListAsync();

            return Ok(trinhDoHocVanList);
        }
    }
}
