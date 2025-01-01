using Web.Dto;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Web.Models;
using BTL.Dto;

namespace Web.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NhanVienController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return null;  // Token không tồn tại hoặc hết hạn
            }

            // Giải mã token JWT để lấy thời gian hết hạn
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenInVerification = jwtTokenHandler.ReadToken(accessToken) as JwtSecurityToken;
            var expireDateClaim = tokenInVerification?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

            if (expireDateClaim != null)
            {
                var expireDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expireDateClaim)).UtcDateTime;
                if (expireDate < DateTime.UtcNow) // Nếu token hết hạn
                {
                    // Gọi API RenewToken để làm mới token
                    var client = _httpClientFactory.CreateClient("ApiClient");
                    var model = new TokenModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    };

                    var response = await client.PostAsJsonAsync("RenewToken", model);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TokenModel>>();

                        if (apiResponse?.Success == true)
                        {
                            // Lưu lại token mới vào session
                            HttpContext.Session.SetString("AccessToken", apiResponse.Data?.AccessToken);
                            HttpContext.Session.SetString("RefreshToken", apiResponse.Data?.RefreshToken);
                            return apiResponse.Data?.AccessToken; // Trả lại token mới
                        }
                        else
                        {
                            // Nếu API trả về lỗi, hiển thị thông báo
                            ViewBag.Error = apiResponse?.Message ?? "Không thể làm mới token. Vui lòng thử lại.";
                            return null;
                        }
                    }
                    else
                    {
                        // Nếu response không thành công, hiển thị lỗi từ API
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        ViewBag.Error = $"Lỗi từ server: {errorMessage}";
                        return accessToken;
                    }
                }
            }

            return accessToken; // Nếu token vẫn còn hiệu lực
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = await GetAccessTokenAsync();

            if (string.IsNullOrEmpty(accessToken))
            {
                TempData["error"] = "Bạn chưa đăng nhập hoặc token đã hết hạn.";
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // Gọi API với HttpGet
            var response = await client.GetAsync("NhanVien/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<List<NhanVienDto>>();

                if (productList != null)
                {
                    return View(productList);  // Truyền dữ liệu vào View
                }
                else
                {
                    // Nếu không có dữ liệu
                    ViewBag.Error = "Không có dữ liệu sản phẩm";
                    return View(new List<NhanVienDto>());
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Lỗi 401 - Unauthorized
                TempData["error"] = "Bạn chưa đăng nhập hoặc token hết hạn.";
                return RedirectToAction("Login", "Auth");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                // Lỗi 403 - Forbidden: Người dùng không có quyền truy cập
                ViewBag.Error = "Bạn không có quyền truy cập vào dữ liệu này.";
                return View(new List<NhanVienDto>());
            }
            else
            {
                // Các trường hợp lỗi khác
                ViewBag.Error = $"Có lỗi xảy ra. Mã lỗi: {response.StatusCode}";
                return View(new List<NhanVienDto>());
            }
        }
        public IActionResult Admin()
        {
            return View();
        }

        // Fetch data for CongViec dropdown
        private async Task<List<CongViecDto>> GetCongViec()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("CongViec");  // URL của API trả về danh sách công việc
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CongViecDto>>();
            }
            return new List<CongViecDto>();
        }

        // Fetch data for PhongBan dropdown
        private async Task<List<PhongBanDto>> GetPhongBan()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("PhongBan");  // URL của API trả về danh sách phòng ban
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PhongBanDto>>();
            }
            return new List<PhongBanDto>();
        }

        // Fetch data for TrinhDoHocVan dropdown
        private async Task<List<TrinhDoHocVanDto>> GetTrinhDoHocVan()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("HocVan");  // URL của API trả về danh sách trình độ học vấn
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TrinhDoHocVanDto>>();
            }
            return new List<TrinhDoHocVanDto>();
        }

        // Fetch data for BacLuong dropdown
        private async Task<List<BacLuongDto>> GetBacLuong()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("Luong");  // URL của API trả về danh sách bậc lương
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<BacLuongDto>>();
            }
            return new List<BacLuongDto>();
        }
        public async Task<IActionResult> Create()

        {
            // Fetch data for dropdowns asynchronously
            ViewBag.CongViec = await GetCongViec();  // Lấy danh sách công việc
            ViewBag.PhongBan = await GetPhongBan();  // Lấy danh sách phòng ban
            ViewBag.TrinhDoHocVan = await GetTrinhDoHocVan();  // Lấy danh sách trình độ học vấn
            ViewBag.BacLuong = await GetBacLuong();  // Lấy danh sách bậc lương
            var model = new NhanVienCreateDto();
            return View(model);
        }
        // POST: NhanVien/Create
        [HttpPost]
 
        public async Task<IActionResult> Create(NhanVienCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                var content = new MultipartFormDataContent();

                // Thêm các trường dữ liệu vào multipart content
                content.Add(new StringContent(model.HoTen), "HoTen");
                content.Add(new StringContent(model.QueQuan), "QueQuan");
                content.Add(new StringContent(model.GioiTinh), "GioiTinh");
                content.Add(new StringContent(model.DienThoai), "DienThoai");
                content.Add(new StringContent(model.DanToc), "DanToc");
                content.Add(new StringContent(model.DiaChi), "DiaChi");
                content.Add(new StringContent(model.Email), "Email");
                content.Add(new StringContent(model.NgaySinh), "NgaySinh");
                content.Add(new StringContent(model.MaCV.ToString()), "MaCV");
                content.Add(new StringContent(model.MaPB.ToString()), "MaPB");
                content.Add(new StringContent(model.MaTDHV.ToString()), "MaTDHV");
                content.Add(new StringContent(model.BacLuong), "BacLuong");

                // Thêm ảnh vào request
                if (model.Anh != null)
                {
                    var fileContent = new StreamContent(model.Anh.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Anh.ContentType);
                    content.Add(fileContent, "Anh", model.Anh.FileName);
                }

                var response = await client.PostAsync("NhanVien/Create", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Thêm thành công";
                    return RedirectToAction("Index");
                    
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Đã có lỗi xảy ra khi tạo nhân viên: {errorContent}");
                }
            }

            // Fetch data for dropdowns in case of invalid model
            ViewBag.CongViec = await GetCongViec();  // Lấy danh sách công việc
            ViewBag.PhongBan = await GetPhongBan();  // Lấy danh sách phòng ban
            ViewBag.TrinhDoHocVan = await GetTrinhDoHocVan();  // Lấy danh sách trình độ học vấn
            ViewBag.BacLuong = await GetBacLuong();  // Lấy danh sách bậc lương
            TempData["error"]= "Thêm thất bại";
            return View(model);
        }

        // POST: NhanVien/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, NhanVienUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                var content = new MultipartFormDataContent();

                // Thêm các trường dữ liệu vào multipart content
                content.Add(new StringContent(model.HoTen), "HoTen");
                content.Add(new StringContent(model.QueQuan), "QueQuan");
                content.Add(new StringContent(model.GioiTinh), "GioiTinh");
                content.Add(new StringContent(model.DienThoai), "DienThoai");
                content.Add(new StringContent(model.DanToc), "DanToc");
                content.Add(new StringContent(model.DiaChi), "DiaChi");
                content.Add(new StringContent(model.Email), "Email");
                content.Add(new StringContent(model.NgaySinh), "NgaySinh");
                content.Add(new StringContent(model.MaCV.ToString()), "MaCV");
                content.Add(new StringContent(model.MaPB.ToString()), "MaPB");
                content.Add(new StringContent(model.MaTDHV.ToString()), "MaTDHV");
                content.Add(new StringContent(model.BacLuong), "BacLuong");

                // Thêm ảnh vào request nếu có
                if (model.Anh != null)
                {
                    var fileContent = new StreamContent(model.Anh.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.Anh.ContentType);
                    content.Add(fileContent, "Anh", model.Anh.FileName);
                }

                // Gửi PUT request đến API
                var response = await client.PutAsync($"NhanVien/Update/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Sửa thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Cập nhật nhân viên thất bại: {errorContent}");
                }
            }

            // Fetch data for dropdowns in case of invalid model
            ViewBag.CongViec = await GetCongViec();  // Lấy danh sách công việc
            ViewBag.PhongBan = await GetPhongBan();  // Lấy danh sách phòng ban
            ViewBag.TrinhDoHocVan = await GetTrinhDoHocVan();  // Lấy danh sách trình độ học vấn
            ViewBag.BacLuong = await GetBacLuong();  // Lấy danh sách bậc lương

            return View(model);
        }

        // POST: NhanVien/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"NhanVien/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "xóa thành công";
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", new ErrorViewModel { RequestId = "Lỗi khi xóa nhân viên." });
            }
        }
        // GET: NhanVien/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"NhanVien/ById/{id}");
            ViewBag.NhanVienId = id;
            if (response.IsSuccessStatusCode)
            {
                var nhanVien = await response.Content.ReadFromJsonAsync<NhanVienDto>();
                if (nhanVien == null)
                {
                    return NotFound();
                }
                return View(nhanVien);
            }

            return RedirectToAction("Index");
        }
        // GET: NhanVien/ByName
        public async Task<IActionResult> ByName(string name)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"NhanVien/ByName/{name}");

            if (response.IsSuccessStatusCode)
            {
                var nhanViens = await response.Content.ReadFromJsonAsync<List<NhanVienDto>>();
                if (nhanViens == null || !nhanViens.Any())
                {
                    ViewBag.ErrorMessage = "Không tìm thấy nhân viên nào.";
                    return View();
                }

                return View(nhanViens);
            }

            return View();
        }

    }
}
