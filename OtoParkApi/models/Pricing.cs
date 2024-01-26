// Pricing.cs

using System;

namespace OtoParkApi
{
    public class Pricing
    {
        public int Id { get; set; }
        public int MinHours { get; set; }
        public int MaxHours { get; set; }
        public decimal Price { get; set; }
    }
}
