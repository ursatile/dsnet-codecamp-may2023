using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace SwashbuckleDemo.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase {
		private static readonly string[] Summaries = new[] {
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger) {
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get() {
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast {	
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
				.ToArray();
		}

		[HttpGet("{id:Guid}")]
		public WeatherForecast Get(Guid id) {
			return new WeatherForecast {
				Id = id,
				Date = DateTime.Now.AddDays(0),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			};
		}

		/// <summary>Add a new forecast to the collection.</summary>
		/// <param name="forecast">The weather forecast you want to add to the system.</param>
		/// <returns>201 Created if successful, otherwise 400 Bad Request</returns>
		[HttpPost]
		[SwaggerResponse(201, "Created", typeof(WeatherForecast), Description = "You created a new forecast! Well done!")]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad request", Description = "That was not OK. Don't do it again")]
		public IActionResult Post([FromBody] WeatherForecast forecast) {
			return Created($"/WeatherForecast/{forecast.Id}", forecast);
		}

		[HttpPut("{id:Guid}")]
		public IActionResult Put(Guid id, [FromBody] WeatherForecast forecast) {
			return Created($"/WeatherForecast/{forecast.Id}", forecast);
		}

		[HttpDelete("{id:Guid}")]
		public IActionResult Delete(Guid id) => NoContent();
	}
}
