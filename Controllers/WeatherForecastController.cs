using Microsoft.AspNetCore.Mvc;
using Quartz.Impl;
using Quartz;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using DotNet.RateLimiter.ActionFilters;

namespace QuartzProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

       
        private readonly IMemoryCache _memory;

        public WeatherForecastController( IMemoryCache memory)
        {
            _memory = memory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            // 2. Get and start a scheduler
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            // 3. Create a job
            IJobDetail job = JobBuilder.Create<TelefonoGenerator>()
                    .WithIdentity("number generator job", "number generator group")
                    .Build();

            // 4. Create a trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("number generator trigger", "number generator group")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                .Build();

            // 5. Schedule the job using the job and trigger 
            await scheduler.ScheduleJob(job, trigger);

            return new string("Hello");
        }

        [HttpGet]
        [Route("/Memory/A")]
        [RateLimit(PeriodInSec = 10, Limit = 3)] //Limitador de solicitudes
        public IEnumerable<string> GetStrings(){


            var cache = _memory.Get<IEnumerable<string>> ("Weather");
            var expirationTime = DateTimeOffset.Now.AddSeconds(10);
            
            if (cache is null) {
                _memory.Set("Weather", Summaries, expirationTime);
                cache = _memory.Get<IEnumerable<string>>("Weather");
                return cache!;
            }

            return cache;
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
