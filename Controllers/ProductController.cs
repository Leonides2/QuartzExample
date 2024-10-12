using DotNet.RateLimiter.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quartz.Impl;
using Quartz;

namespace QuartzProject.Controllers
{
    public class ProductController : Controller
    {
        [ApiController]
        [Route("[controller]")]
        public class WeatherForecastController : ControllerBase
        {

            private readonly IMemoryCache _memory;

            public WeatherForecastController(IMemoryCache memory)
            {
                _memory = memory;
            }

            [HttpGet(Name = "GetWeatherForecast")]
            public async Task<string> Get()
            {

                return new string("Hello");
            }


            [HttpPost]
            public string Post([FromBody] string[] entry)
            {

                _memory.Remove("Weather");
                Summaries.Concat(entry);

                return new string($"success adding {entry}");

            }
        }
    }
}
