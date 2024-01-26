using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using OtoParkFrontend.Models; // Adjust the namespace based on your project structure

namespace OtoParkFrontend.Controllers
{
    public class OtoParkController : Controller
    {
        private readonly HttpClient _httpClient;

        public OtoParkController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5268/"); // Replace with your API base URL
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            
           var response = await _httpClient.GetAsync("OtoPark/allVehicles");


            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ssZ") }
                };

            var vehicles = JsonSerializer.Deserialize<List<Car>>(content, options);

            return View(vehicles);

        }

        // Other actions for interacting with the API...

        public IActionResult Error()
        {
            return View();
        }
    }
}
