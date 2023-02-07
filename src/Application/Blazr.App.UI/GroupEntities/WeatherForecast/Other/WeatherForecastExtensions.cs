using Blazr.App.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazr.App.UI;

public static class WeatherForecastExtensions
{
    public static string AsTemperatureF(this WeatherForecast item)
        => ((item.TemperatureC * 9 / 5) + 32).ToString();
}
