using Bill_UI.DTOs;
using Bill_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Policy;

namespace Bill_UI.Controllers
{
    public class HomeController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }
        [HttpPost]
        public JsonResult GetProductList()
        {
            string url = "https://localhost:7249/api/ProductAPI/";
            List<Product> products = new List<Product>();
            HttpResponseMessage response = _client.GetAsync(url + "GetProducts").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Product>>(result);
                if (data != null)
                {
                    products = data;
                }
            }
            return Json(products);
        }

        [HttpPost]
        public JsonResult GetDetailsJson(int id)
        {
            string url = "https://localhost:7249/api/InventoryAPI/";
            InventoryDto inventoryDto = new InventoryDto();
            HttpResponseMessage response = _client.GetAsync($"{url}GetInfoByBillNo/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<InventoryDto>(result);
                if (data != null)
                {
                    inventoryDto = data;
                }
            }

            return Json(inventoryDto);
        }
        [HttpPost]
        public async Task<IActionResult> EditDetail([FromBody] InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
            {
                return BadRequest();
            }
            var productsWithoutId = inventoryDto.InventoryProducts.Select(p => new
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Rate = p.Rate,
                Qty = p.Qty,
                Discount = p.Discount
            }).ToList();

            // Create a new inventoryDto without the Id field in InventoryProducts
            var modifiedInventoryDto = new
            {
                Id = inventoryDto.Id,
                Date = inventoryDto.Date,
                BillNo = inventoryDto.BillNo,
                CustomerId = inventoryDto.CustomerId,
                CustomerName = inventoryDto.CustomerName,
                TotalDiscount = inventoryDto.TotalDiscount,
                TotalBillAmount = inventoryDto.TotalBillAmount,
                DueAmount = inventoryDto.DueAmount,
                PaidAmount = inventoryDto.PaidAmount,
                InventoryProducts = productsWithoutId
            };
            string url = "https://localhost:7249/api/InventoryAPI/";
            string data = JsonConvert.SerializeObject(modifiedInventoryDto);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PutAsync(url + inventoryDto.Id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["update_i nventory"] = "Inventory Updated...";
                return RedirectToAction("Index");
            }
            else
            {
                var errorResponse = response.Content.ReadAsStringAsync().Result;
                return BadRequest($"Failed to update: {errorResponse}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var combinedViewModel = new CombinedViewModel
            {
                CustomersNames = await GetCustomersNames(),
                ProductNames = await GetProductNames()
            };

            return View(combinedViewModel);
        }

        private async Task<CustomersNames> GetCustomersNames()
        {
            string url = "https://localhost:7249/api/CustomerAPI/GetCustomersNames";
            CustomersNames cNames = new CustomersNames();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<CustomersNames>(result);
                if (data != null)
                {
                    cNames = data;
                }
            }
            return cNames;
        }

        private async Task<ProductNames> GetProductNames()
        {
            string url = "https://localhost:7249/api/ProductAPI/GetProductNames";
            ProductNames pNames = new ProductNames();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductNames>(result);
                if (data != null)
                {
                    pNames = data;
                }
            }
            return pNames;
        }

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		[HttpPost]
		public int Add(int num1, int num2)
		{
			return num1 + num2;
		}
		public int Subtract(int num1, int num2)
		{
			return num1 - num2;
		}
        public Calculation Calculate(int num1, int num2)
        {
			Calculation mod = new Calculation();
			mod.Add = num1 + num2;
			mod.Subtract = num1 - num2;
			mod.Multiply = num1 * num2;
			mod.Division = (double)num1 / num2;
			return mod;
        }
    }
}