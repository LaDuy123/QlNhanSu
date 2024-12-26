using BTL.Data;
using BTL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private MyDbContext _context;

        public AuthController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                // Trả về ID của người dùng thành công
                return Ok(user.Id);
            }

            return BadRequest("Đăng nhập không thành công");
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            // Ví dụ: Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = _context.Admins.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Trả về thông tin người dùng dưới dạng đối tượng JSON
            var userInfo = new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return Ok(userInfo);
        }
    }
}
