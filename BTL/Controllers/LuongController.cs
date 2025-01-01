using BTL.Data;
using BTL.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuongController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LuongController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/BacLuong
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BacLuongDto>>> GetBacLuong()
        {
            var bacLuongList = await _context.Luongs
                                             .Select(bl => new BacLuongDto
                                             {
                                                 BacLuong = bl.BacLuong,

                                             })
                                             .ToListAsync();

            return Ok(bacLuongList);
        }
    }
}
