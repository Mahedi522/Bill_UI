using Bill_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bill_UI.Controllers
{
	public class CustomerController : Controller
	{
		private string url = "https://localhost:7249/api/CustomerAPI/";
		private HttpClient client = new HttpClient();

		[HttpGet]
		public IActionResult GetCustomers()
		{
			List<Customer> customers = new List<Customer>();
			HttpResponseMessage response = client.GetAsync(url+ "GetCustomers").Result;
			if (response.IsSuccessStatusCode)
			{
				string result = response.Content.ReadAsStringAsync().Result;
				var data = JsonConvert.DeserializeObject<List<Customer>>(result);
				if (data != null)
				{
					customers = data;
				}
			}
			return View(customers);
		}
        [HttpGet]
        public IActionResult Details(int id)
        {
            Customer customer = new Customer();
            HttpResponseMessage response = client.GetAsync($"{url}GetCustomerById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Customer>(result);
                if (data != null)
                {
                    customer = data;
                }
            }
            return View(customer);

        }
        [HttpGet]
        public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Customer customer)
		{
			string data = JsonConvert.SerializeObject(customer);
			StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = client.PostAsync(url+ "CreateCustomer", content).Result;
			if (response.IsSuccessStatusCode)
			{
                TempData["insert_message"] = "Customer Added...";
                return RedirectToAction("GetCustomers");
			}
			return View();

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Customer customer = new Customer();
            HttpResponseMessage response = client.GetAsync($"{url}GetCustomerById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Customer>(result);
                if (data != null)
                {
                    customer = data;
                }
            }
            return View(customer);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            string data = JsonConvert.SerializeObject(customer);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + customer.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Customer Updated...";
                return RedirectToAction("GetCustomers");
            }
            return View();

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customer customer = new Customer();
            HttpResponseMessage response = client.GetAsync($"{url}GetCustomerById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Customer>(result);
                if (data != null)
                {
                    customer = data;
                }
            }
            return View(customer);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = client.DeleteAsync($"{url}DeleteCustomer/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["delete_message"] = "Customer Deleted";
                return RedirectToAction("GetCustomers");
            }
            return View();
        }
    }
}
