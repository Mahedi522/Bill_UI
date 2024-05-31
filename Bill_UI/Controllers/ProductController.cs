using Bill_UI.DTOs;
using Bill_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bill_UI.Controllers
{
    public class ProductController : Controller
    {
        private string url = "https://localhost:7249/api/ProductAPI/";
        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();
            HttpResponseMessage response = client.GetAsync(url + "GetProducts").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Product>>(result);
                if (data != null)
                {
                    products = data;
                }
            }
            return View(products);
        }
        [HttpPost]
        public JsonResult GetProductList()
        {
            List<Product> products = new List<Product>();
            HttpResponseMessage response = client.GetAsync(url + "GetProducts").Result;
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
        [HttpGet]
        public IActionResult Details(int id)
        {
            Product product = new Product();
            HttpResponseMessage response = client.GetAsync($"{url}GetProductById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Product>(result);
                if (data != null)
                {
                    product = data;
                }
            } 
            return View(product);

        }
        [HttpPost]
        public JsonResult GetProductJson(int id)
        {
            Product product = new Product();
            HttpResponseMessage response = client.GetAsync($"{url}GetProductById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Product>(result);
                if (data != null)
                {
                    product = data;
                }
            }

            return Json(product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url + "CreateProduct", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["insert_message"] = "Product Added...";
                return RedirectToAction("GetProducts");
            }
            return View();

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = new Product();
            HttpResponseMessage response = client.GetAsync($"{url}GetProductById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Product>(result);
                if (data != null)
                {
                    product = data;
                }
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            string data = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + product.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Product Updated...";
                return RedirectToAction("GetProducts");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Product product = new Product();
            HttpResponseMessage response = client.GetAsync($"{url}GetProductById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Product>(result);
                if (data != null)
                {
                    product = data;
                }
            }
            return View(product);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = client.DeleteAsync($"{url}DeleteProduct/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["delete_message"] = "Product Deleted";
                return RedirectToAction("GetProducts");
            }
            return View();
        }
    }
}
