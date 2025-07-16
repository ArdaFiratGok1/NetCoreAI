using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project2_ApiConsumeUI.Dtos;
using Newtonsoft.Json;

namespace NetCoreAI.Project2_ApiConsumeUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CustomerList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7264/api/Customers");
            if(responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(jsonData);
                return View(values);

            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto customer)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData =JsonConvert.SerializeObject(customer);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7264/api/Customers", stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7264/api/Customers/GetCustomer?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<GetByIdCustomerDto>(jsonData);//Json bir dizi olarak convert ediliyor ondan dolayı List olarak tanımlıyorum.
                return View(values);

            }

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updated_customer)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData=JsonConvert.SerializeObject(updated_customer);
            StringContent stringContent = new StringContent(jsonData,Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7264/api/Customers",stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7264/api/Customers?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerList");
            }
            return View();
        }
    }
}
