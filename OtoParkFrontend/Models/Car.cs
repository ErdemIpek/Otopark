using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Car
{
    public int Id { get; set; }
    public string LicensePlate { get; set; }="";
    public DateTime EntryTime { get; set; }=DateTime.MinValue;
    public DateTime? LeaveTime { get; set; } = null; 
}
public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format;

    public JsonDateTimeConverter(string format)
    {
        _format = format;
    }

  public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    if (reader.TokenType == JsonTokenType.Null)
    {
        // Handle the case where the date value is null
        return DateTime.MinValue; // or another appropriate default value
    }

    if (reader.TryGetDateTime(out DateTime dateTime))
    {
        return dateTime;
    }

    // Handle the case where the date string is not a valid DateTime
    return DateTime.MinValue; // or another appropriate default value
}


    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format, CultureInfo.InvariantCulture));
    }
}
