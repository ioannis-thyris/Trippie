using Trippie.Database;
using Trippie.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Trippie.Controllers
{
    public class PricingController : Controller
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly string baseUrl = "https://api-m.sandbox.paypal.com";
        private readonly string currency = "EUR";
        private readonly string price = "4.99";
        private readonly string clientId = "AXi2EH1EWGblH_H_B_b7xsvUZWifWYn7fB4afJDmtdHtsjX7Q1zh3UG1T63ECvimyzvSDHzyXYuVJK7L";
        private readonly string appSecret = "EKROHEfPacYSBEPNT0JfCbnu0_3_C_ut_wTzszcgmEGfI5YoYfbPLnac2jD_XfO6nwKkcU5aWQuWaKrD";

        private readonly ApplicationDbContext _context;
        private readonly UserRepository userRepo;

        public PricingController()
        {
            _context = new ApplicationDbContext();

            userRepo = new UserRepository(_context);
        }


        // GET: Pricing
        public ActionResult Index()
        {
            var isRegistered = User.Identity.IsAuthenticated;

            ViewBag.IsRegistered = isRegistered;

            var user = userRepo.GetCurrentUser(User);

            if (user != null)
            {
                var isPremiumUser = user.IsPremiumUser;

                ViewBag.IsPremiumUser = isPremiumUser;
            }

            return View();
        }

        [HttpPost]
        public async Task<dynamic> CreateOrder()
        {
            string accessToken = await GenerateAccessToken();

            var request = CreateOrderRequest(accessToken);

            var response = await _client.SendAsync(request);

            var jsonString = await response.Content.ReadAsStringAsync();

            var order = JsonConvert.DeserializeObject<dynamic>(jsonString);

            return order;
        }

        [HttpPost]
        public async Task<ActionResult> CapturePayment(string orderId)
        {
            string accessToken = await GenerateAccessToken();

            var request = CapturePaymentRequest(accessToken, orderId);

            var response = await _client.SendAsync(request);

            if(!response.IsSuccessStatusCode)
            {
                return Json(response);
            }

            var user = userRepo.GetCurrentUser(User);

            userRepo.MakeUserPremium(user);

            var succes = new { paymentSuccess = true };

            return Json(new { redirectToUrl = Url.Action("Index", "Home", succes) });
        }


        [NonAction]
        private async Task<string> GenerateAccessToken()
        {
            var request = AccessTokenRequest(clientId, appSecret);

            var response = await _client.SendAsync(request);

            var jsonString = await response.Content.ReadAsStringAsync();

            var parsedData = JsonConvert.DeserializeObject<dynamic>(jsonString);

            return parsedData.access_token;
        }

        [NonAction]
        public HttpRequestMessage AccessTokenRequest(string clientId, string appSecret)
        {
            string url = $"{baseUrl}/v1/oauth2/token";

            var bytes = Encoding.UTF8.GetBytes($"{clientId}:{appSecret}");
            string auth = Convert.ToBase64String(bytes);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

            Dictionary<string, string> body = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            };

            request.Content = new FormUrlEncodedContent(body);

            return request;
        }

        [NonAction]
        private HttpRequestMessage CapturePaymentRequest(string accessToken, string orderId)
        {
            string url = $"{baseUrl}/v2/checkout/orders/{orderId}/capture";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent("");

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }

        [NonAction]
        private HttpRequestMessage CreateOrderRequest(string accessToken)
        {
            string url = $"{baseUrl}/v2/checkout/orders";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var purchasingUnit = new
            {
                amount = new
                {
                    currency_code = currency,
                    value = price
                }
            };

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new List<dynamic> { purchasingUnit }

            };

            request.Content = JsonContent.Create(body);

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}