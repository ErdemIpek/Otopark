using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtoParkApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OtoParkController : ControllerBase
    {
      private readonly OtoParkContext _context;

        public OtoParkController(OtoParkContext context)
        {
            _context = context;
        }

        private Car car; // Declare car at the class level



        [HttpPost("carEntered")]
        public async Task<ActionResult> CarEntered(string licensePlate, DateTime entryTime)
        {
              car = new Car { LicensePlate = licensePlate, EntryTime = entryTime, LeaveTime = null };
              
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPost("carLeft")]
        public async Task<ActionResult<decimal>> CarLeft(string licensePlate, DateTime leaveTime)
        {
            car = await _context.Cars.FirstOrDefaultAsync(c => c.LicensePlate == licensePlate);

            if (car == null)
            {
                return NotFound();
            }

            car.LeaveTime = leaveTime;
            var parkedTime = car.LeaveTime - car.EntryTime;
            var price = CalculatePrice(parkedTime);

            await _context.SaveChangesAsync();

            return Ok(new { TotalPrice = price });
        }

        [HttpGet("allVehicles")]
        public async Task<ActionResult<IEnumerable<Car>>> GetAllVehicles()
        {
            var vehicles = await _context.Cars.ToListAsync();
            return Ok(vehicles);
        }
        [HttpPost("editLicensePlate")]
        public async Task<ActionResult> EditLicensePlate([FromBody] EditLicensePlateRequest request)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.LicensePlate == request.LicensePlate && c.EntryTime == request.EntryTime);

            if (car == null)
            {
                return NotFound($"Entry not found with the license plate: {request.LicensePlate} and entry time: {request.EntryTime}");
            }

            car.LicensePlate = request.NewLicensePlate;

            await _context.SaveChangesAsync();

            return Ok($"License plate updated to: {request.NewLicensePlate} for the entry with license plate: {request.LicensePlate} and entry time: {request.EntryTime}.");
        }

        // Create a model class for the request parameters
        public class EditLicensePlateRequest
        {
            public string LicensePlate { get; set; }
            public DateTime EntryTime { get; set; }
            public string NewLicensePlate { get; set; }
        }
        [HttpPost("updatePrices")]
        public async Task<ActionResult> UpdatePrices(List<Pricing> prices)
        {
            // Assuming you want to update prices for existing entries and add new ones
            foreach (var price in prices)
            {
                var existingPrice = _context.Pricings.Find(price.Id);

                if (existingPrice != null)
                {
                    // Update existing price
                    existingPrice.MinHours = price.MinHours;
                    existingPrice.MaxHours = price.MaxHours;
                    existingPrice.Price = price.Price;
                }
                else
                {
                    // Add new price
                    _context.Pricings.Add(price);
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

       private decimal CalculatePrice(TimeSpan? parkedTime)
{
    // Check if parkedTime is not null
    if (parkedTime.HasValue)
    {
        // Retrieve pricing based on total hours
        int totalHours = (int)Math.Ceiling(parkedTime.Value.TotalHours);
        var pricing = _context.Pricings
            .Where(p => p.MinHours <= totalHours && p.MaxHours >= totalHours)
            .FirstOrDefault();

        if (pricing == null)
        {
            // Handle the case when pricing is not found for the given duration
            return -1; // Adjust this value or handle the case appropriately
        }

        // Calculate the price
        decimal totalPrice = pricing.Price;

        // Log to console (you can replace this with your actual logging mechanism)
        Console.WriteLine($"License Plate: {car.LicensePlate}, Total Price: {totalPrice} TRY");

        return totalPrice;
    }
    else
    {
        // Handle the case where parkedTime is null
        return 0; // Adjust this value or handle the case appropriately
    }
}

    }
}
