using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
	public class AuthController : Controller
	{
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            // Kiểm tra nếu thông tin người dùng đã nhập hợp lệ
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                try
                {
                    // Gửi thông tin đăng nhập đến API
                    var response = await client.PostAsJsonAsync("https://localhost:44314/api/Auth/Login", model);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<TokenModel>>();

                        if (apiResponse != null && apiResponse.Success)
                        {
                            // Lưu token vào session
                            HttpContext.Session.SetString("AccessToken", apiResponse.Data?.AccessToken);
                            HttpContext.Session.SetString("RefreshToken", apiResponse.Data?.RefreshToken);

                            // Chuyển hướng người dùng đến trang chính hoặc trang có quyền truy cập
                            return RedirectToAction("Index", "NhanVien");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = apiResponse?.Message ?? "Login failed!";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Login failed! Please check your credentials.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }

            // Nếu không hợp lệ, trả lại form đăng nhập với lỗi
            return View(model);
        }
    }
}
