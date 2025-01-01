using BTL.Data;
using BTL.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongViecController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CongViecController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/CongViec
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CongViecDto>>> GetCongViec()
        {
            var congViecList = await _context.CongViecs
                                             .Select(cv => new CongViecDto
                                             {
                                                 MaCV = cv.MaCV,
                                                 TenCV = cv.TenCV
                                             })
                                             .ToListAsync();

            return Ok(congViecList);
        }
    }
}
