using BTL.Data;
using BTL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private MyDbContext _context;

        public NhanVienController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var nhanViens = _context.NhanViens
                .Include(nv => nv.CongViec)
                .Include(nv => nv.PhongBan)
                .ToList();

            var nhanVienDTOs = nhanViens.Select(nv => new NhanVienDTO
            {
                MaNV = nv.MaNV,
                HoTen = nv.HoTen,
                DiaChi = nv.DiaChi,
                DienThoai = nv.DienThoai,
                Email = nv.Email,
                NgaySinh = nv.NgaySinh,
                Luong = nv.Luong,
                Anh = nv.Anh,
                MaCongViec = nv.MaCongViec,
                MaPhongBan = nv.MaPhongBan,
                CongViec = nv.CongViec?.TenCongViec,
                PhongBan = nv.PhongBan?.TenPhongBan
            });

            return Ok(nhanVienDTOs);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var nv = _context.NhanViens
                .Include(n => n.CongViec)
                .Include(n => n.PhongBan)
                .SingleOrDefault(n => n.MaNV == id);

            if (nv != null)
            {
                var nvDTO = new NhanVienDTO
                {
                    MaNV = nv.MaNV,
                    HoTen = nv.HoTen,
                    DiaChi = nv.DiaChi,
                    DienThoai = nv.DienThoai,
                    Email = nv.Email,
                    NgaySinh = nv.NgaySinh,
                    Luong = nv.Luong,
                    Anh = nv.Anh,
                    CongViec = nv.CongViec?.TenCongViec,
                    PhongBan = nv.PhongBan?.TenPhongBan
                };

                return Ok(nvDTO);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var nvs = _context.NhanViens
                .Include(n => n.CongViec)
                .Include(n => n.PhongBan)
                .Where(n => n.HoTen.Contains(name))
                .Select(n => new NhanVienDTO
                {
                    MaNV = n.MaNV,
                    HoTen = n.HoTen,
                    DiaChi = n.DiaChi,
                    DienThoai = n.DienThoai,
                    Email = n.Email,
                    NgaySinh = n.NgaySinh,
                    Luong = n.Luong,
                    Anh = n.Anh,
                    CongViec = n.CongViec != null ? n.CongViec.TenCongViec : null,
                    PhongBan = n.PhongBan != null ? n.PhongBan.TenPhongBan : null
                })
                .ToList();

            if (nvs.Count > 0)
            {
                return Ok(nvs);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("Create")]
        public IActionResult Create(NhanVienModel model)
        {
            try
            {
                var nv = new NhanVien
                {
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    NgaySinh = model.NgaySinh,
                    MaCongViec = model.MaCongViec,
                    MaPhongBan = model.MaPhongBan,
                    Luong = model.Luong,
                    Anh = model.Anh,
                };
                _context.Add(nv);
                _context.SaveChanges();
                return Ok(nv);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Edit(int id, NhanVienModel model)
        {
            var nv = _context.NhanViens.SingleOrDefault(l => l.MaNV == id);
            if (nv != null)
            {
                nv.HoTen = model.HoTen;
                nv.DiaChi = model.DiaChi;
                nv.DienThoai = model.DienThoai;
                nv.Email = model.Email;
                nv.NgaySinh = model.NgaySinh;
                nv.MaCongViec = model.MaCongViec;
                nv.MaPhongBan = model.MaPhongBan;
                nv.Luong = model.Luong;
                nv.Anh = model.Anh;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var nv = _context.NhanViens.SingleOrDefault(l => l.MaNV == id);
            if (nv != null)
            {
                _context.NhanViens.Remove(nv);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
