using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace HttpDemo.Client;

public class MainWindowModel : BaseWindowModel
{
    private static HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:44335"),
    };
    
    public ObservableCollection<WeatherForecast> Forecasts { get; } = [];
    
    private WeatherForecast? _selectedForecast;
    public WeatherForecast? SelectedForecast
    {
        get => _selectedForecast;
        set
        {
            var result = SetField(ref _selectedForecast, value);
            if (!result) return;
            
            Date = _selectedForecast?.Date;
            Summary = _selectedForecast?.Summary;
            TemperatureC = _selectedForecast?.TemperatureC;
            TemperatureF = _selectedForecast?.TemperatureF;
        } 
    }

    private DateOnly? _date;
    public DateOnly? Date
    {
        get => _date;
        set => SetField(ref _date, value);
    }
    
    private string? _summary;
    public string? Summary
    {
        get => _summary;
        set => SetField(ref _summary, value);
    }
    
    private int? _temperatureC;
    public int? TemperatureC
    {
        get => _temperatureC;
        set => SetField(ref _temperatureC, value);
    }
    
    private int? _temperatureF;
    public int? TemperatureF
    {
        get => _temperatureF;
        set => SetField(ref _temperatureF, value);
    }
    
    public LambdaCommand CommandRefresh {get; }

    /*public MainWindowModel()
    {
        CommandRefresh = new LambdaCommand(async _ => await RefreshAsync(null));
    }

    private async Task RefreshAsync(object? obj)
    {
        var result = await _httpClient
            .GetFromJsonAsync<WeatherForecast[]>("weatherforecast");
        
        Forecasts.Clear();
        foreach (var forecast in result)
        {
            Forecasts.Add(forecast);
        }
    }*/

    public MainWindowModel()
    {
        CommandRefresh = new LambdaCommand(Refresh);
    }
    
    private async void Refresh(object? obj)
    {
        try
        {
            var result = _httpClient
                .GetFromJsonAsAsyncEnumerable<WeatherForecast>("weatherforecast");
        
            Forecasts.Clear();
            await foreach (var forecast in result)
            {
                if(forecast is null) continue;
            
                Forecasts.Add(forecast);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(
                caption:"Error", 
                messageBoxText: "Network error",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
        }
    }
}