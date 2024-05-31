using Bill_UI.DTOs;
using Bill_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Bill_UI.Controllers
{
    public class InventoryController : Controller
    {
        private readonly string url = "https://localhost:7249/api/InventoryAPI/";
        private readonly HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<Inventory> inventories = new List<Inventory>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Inventory>>(result);
                if (data != null)
                {
                    inventories = data;
                }
            }
            return View(inventories);
        }
        [HttpGet]
        public IActionResult GetInventoryById(int id)
        {
            Inventory inventory = new Inventory();
            HttpResponseMessage response = client.GetAsync($"{url}GetInventoryById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Inventory>(result);
                if (data != null)
                {
                    inventory = data;
                }
            }
            return View(inventory);
        }
        [HttpPost]
        public IActionResult Edit(Inventory inventory)
        {
            string data = JsonConvert.SerializeObject(inventory);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + inventory.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Product Updated...";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            InventoryDto inventoryDto = new InventoryDto();
            HttpResponseMessage response = client.GetAsync($"{url}GetInfoByBillNo/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<InventoryDto>(result);
                if (data != null)
                {
                    inventoryDto = data;
                }
            }
            return View(inventoryDto); // Return the view
        }


        //[HttpPost]
        //public JsonResult GetDetailsJson(int id)
        //{
        //    InventoryDto inventoryDto = new InventoryDto();
        //    HttpResponseMessage response = client.GetAsync($"{url}GetInfoByBillNo/{id}").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string result = response.Content.ReadAsStringAsync().Result;
        //        var data = JsonConvert.DeserializeObject<InventoryDto>(result);
        //        if (data != null)
        //        {
        //            inventoryDto = data;
        //        }
        //    }

        //    var resultDto = new
        //    {
        //        inventoryDto.Id,
        //        inventoryDto.BillNo,
        //        DateTime = inventoryDto.Date.ToString("yyyy-MM-dd HH:mm:ss"), // Format DateTime as string
        //        inventoryDto.CustomerId,
        //        inventoryDto.CustomerName,
        //        inventoryDto.TotalDiscount,
        //        inventoryDto.TotalBillAmount,
        //        inventoryDto.PaidAmount,
        //        inventoryDto.InventoryProducts
        //    };

        //    return Json(resultDto);
        //}
        //[HttpPost]
        //public JsonResult GetProductList()
        //{
        //    string url = "https://localhost:7249/api/ProductAPI/";
        //    List<Product> products = new List<Product>();
        //    HttpResponseMessage response = client.GetAsync(url + "GetProducts").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string result = response.Content.ReadAsStringAsync().Result;
        //        var data = JsonConvert.DeserializeObject<List<Product>>(result);
        //        if (data != null)
        //        {
        //            products = data;
        //        }
        //    }
        //    return Json(products);
        //}


    }
}
