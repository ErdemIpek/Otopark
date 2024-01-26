// Car.cs

using System;
namespace OtoParkApi
{
public class Car
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }
    public DateTime EntryTime { get; set; }=DateTime.MinValue;
    public DateTime? LeaveTime { get; set; } = null; 
}
}