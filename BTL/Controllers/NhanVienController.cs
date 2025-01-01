using BTL.Data;
using BTL.Dto;
using BTL.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            // Lấy danh sách nhân viên, bao gồm các thông tin từ các bảng liên quan
            var nhanViens = _context.NhanViens
                .Include(nv => nv.CongViec)
                .Include(nv => nv.PhongBan)
                .Include(nv => nv.TrinhDoHocVan)
                .Include(nv => nv.luong)
                .ToList();

            // Ánh xạ từ NhanVien sang NhanVienDTO
            var nhanVienDTOs = nhanViens.Select(nv => new NhanVienDto
            {
                MaNV = nv.MaNV,
                HoTen = nv.HoTen,
                QueQuan = nv.QueQuan,
                GioiTinh = nv.GioiTinh,
                DienThoai = nv.DienThoai,
                DanToc = nv.DanToc,
                DiaChi = nv.DiaChi,
                Email = nv.Email,
                NgaySinh = nv.NgaySinh,
                MaCV = nv.MaCV,
                MaPB = nv.MaPB,
                MaTDHV = nv.MaTDHV,
                BacLuong = nv.BacLuong,
                Anh = nv.Anh,
                CongViec = nv.CongViec?.TenCV,
                PhongBan = nv.PhongBan?.TenPB,
                TrinhDoHocVan = nv.TrinhDoHocVan?.TenTDHV,
                Luong = nv.luong?.CalculatedTongLuong,
            });

            // Trả về kết quả dưới dạng JSON
            return Ok(nhanVienDTOs);
        }


        [HttpGet("ById/{id}")]
        public IActionResult GetNhanVienById(int id)
        {
            var nhanVien = _context.NhanViens.FirstOrDefault(n => n.MaNV == id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return Ok(nhanVien);
        }
        [HttpGet("ByName/{name}")]
        public IActionResult GetNhanVienByName(string name)
        {
            var nhanViens = _context.NhanViens
                                    .Where(n => n.HoTen.Contains(name))
                                    .ToList();

            if (nhanViens == null || nhanViens.Count == 0)
            {
                return NotFound(new { message = "Không tìm thấy nhân viên nào." });
            }

            return Ok(nhanViens);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateNhanVien(NhanVienCreateDto model)
        {
            if (model == null || model.Anh == null)
            {
                return BadRequest("Dữ liệu không hợp lệ hoặc ảnh chưa được chọn.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = null;

                    // Lưu ảnh vào thư mục wwwroot/images
                    if (model.Anh != null)
                    {
                        var folderPath = Path.Combine("wwwroot", "images");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Giữ nguyên tên file ảnh
                        fileName = model.Anh.FileName;

                        var filePath = Path.Combine(folderPath, fileName);

                        // Lưu file và ghi đè nếu đã tồn tại
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Anh.CopyToAsync(fileStream);
                        }
                    }


                    // Chuyển đổi từ DTO sang entity
                    var nhanVien = new NhanVien
                    {
                        HoTen = model.HoTen,
                        QueQuan = model.QueQuan,
                        GioiTinh = model.GioiTinh,
                        DienThoai = model.DienThoai,
                        DanToc = model.DanToc,
                        DiaChi = model.DiaChi,
                        Email = model.Email,
                        NgaySinh = model.NgaySinh,
                        MaCV = model.MaCV,
                        MaPB = model.MaPB,
                        MaTDHV = model.MaTDHV,
                        BacLuong = model.BacLuong,
                        Anh = fileName // Lưu đường dẫn ảnh trong cơ sở dữ liệu
                    };

                    // Thêm nhân viên vào cơ sở dữ liệu
                    _context.NhanViens.Add(nhanVien);
                    await _context.SaveChangesAsync();

                    // Trả về thông báo thành công
                    return CreatedAtAction("GetNhanVienById", new { id = nhanVien.MaNV }, nhanVien);
                }
                catch (Exception ex)
                {
                    // Trả về lỗi nếu xảy ra vấn đề trong quá trình xử lý
                    return StatusCode(500, "Đã xảy ra lỗi khi thêm nhân viên: " + ex.Message);
                }
            }

            // Trả về lỗi nếu model không hợp lệ
            return BadRequest(ModelState);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateNhanVien(int id, NhanVienUpdateDto model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Kiểm tra xem nhân viên có tồn tại không
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound($"Nhân viên với ID {id} không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = nhanVien.Anh; // Giữ nguyên tên file ảnh cũ nếu không có thay đổi ảnh

                    // Kiểm tra và cập nhật ảnh mới nếu có
                    if (model.Anh != null)
                    {
                        var folderPath = Path.Combine("wwwroot", "images");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Lưu ảnh mới, ghi đè nếu đã tồn tại
                        fileName = model.Anh.FileName;
                        var filePath = Path.Combine(folderPath, fileName);

                        // Xóa ảnh cũ nếu có ảnh mới (nếu cần)
                        var oldFilePath = Path.Combine(folderPath, nhanVien.Anh);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Anh.CopyToAsync(fileStream);
                        }
                    }

                    // Cập nhật thông tin nhân viên (chỉ cập nhật nếu có giá trị mới)
                    nhanVien.HoTen = model.HoTen ?? nhanVien.HoTen;
                    nhanVien.QueQuan = model.QueQuan ?? nhanVien.QueQuan;
                    nhanVien.GioiTinh = model.GioiTinh ?? nhanVien.GioiTinh;
                    nhanVien.DienThoai = model.DienThoai ?? nhanVien.DienThoai;
                    nhanVien.DanToc = model.DanToc ?? nhanVien.DanToc;
                    nhanVien.DiaChi = model.DiaChi ?? nhanVien.DiaChi;
                    nhanVien.Email = model.Email ?? nhanVien.Email;
                    nhanVien.NgaySinh = model.NgaySinh ?? nhanVien.NgaySinh;
                    nhanVien.MaCV = model.MaCV ?? nhanVien.MaCV;
                    nhanVien.MaPB = model.MaPB ?? nhanVien.MaPB;
                    nhanVien.MaTDHV = model.MaTDHV ?? nhanVien.MaTDHV;
                    nhanVien.BacLuong = model.BacLuong ?? nhanVien.BacLuong;
                    nhanVien.Anh = fileName; // Cập nhật ảnh mới nếu có

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.NhanViens.Update(nhanVien);
                    await _context.SaveChangesAsync();

                    // Trả về thông báo thành công
                    return Ok(nhanVien);
                }
                catch (Exception ex)
                {
                    // Trả về lỗi nếu có vấn đề trong quá trình xử lý
                    return StatusCode(500, "Đã xảy ra lỗi khi cập nhật nhân viên: " + ex.Message);
                }
            }

            // Trả về lỗi nếu model không hợp lệ
            return BadRequest(ModelState);
        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var nhanVien = _context.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return NotFound(new { message = "Nhân viên không tồn tại." });
            }

            _context.NhanViens.Remove(nhanVien);
            _context.SaveChanges();

            return Ok(new { message = "Nhân viên đã được xóa thành công." });
        }
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            return null;
        }

    }
}
