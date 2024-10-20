using System.Text.Json.Serialization;

namespace HttpDemo.Client;

public record WeatherForecast(
    [property: JsonPropertyName("date")] DateOnly Date,
    [property: JsonPropertyName("temperatureC")] int? TemperatureC,
    [property: JsonPropertyName("summary")] string Summary,
    [property: JsonPropertyName("temperatureF")] int? TemperatureF
);

