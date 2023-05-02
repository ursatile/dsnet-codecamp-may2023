using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase {
	private readonly IAutobarnDatabase db;

	public VehiclesController(IAutobarnDatabase db) {
		this.db = db;
	}

	private const int PAGE_SIZE = 10;
	[HttpGet]
	[Produces("application/hal+json")]
	public IActionResult Get(int index = 0) {
		var vehicles = db.ListVehicles().Skip(index).Take(PAGE_SIZE);
		var total = db.CountVehicles();
		dynamic _links = new ExpandoObject();
		if (index > 0) {
			_links.previous = new {
				href = $"/api/vehicles?index={index - PAGE_SIZE}"
			};
		}
		if (index + PAGE_SIZE <= total) {
			_links.next = new {
				href = $"/api/vehicles?index={index + PAGE_SIZE}"
			};
		}

		var result = new {
			_links,
			total,
			index,
			count = PAGE_SIZE,
			_items = vehicles.Select(vehicle => vehicle.ToResource())
		};
		return Ok(result);
	}

	// GET api/<VehiclesController>/5
	[Produces("application/hal+json")]
	[HttpGet("{id}")]
	public IActionResult Get(string id) {
		var vehicle = db.FindVehicle(id);
		if (vehicle == default) return NotFound();
		return Ok(vehicle.ToResource());
	}

	// POST api/<VehiclesController>
	[HttpPost]
	public IActionResult Post([FromBody] VehicleDto dto) {
		var model = db.FindModel(dto.ModelCode);
		if (model == default) return BadRequest($"Sorry, model code {dto.ModelCode} was not found in our database.");
		var v = new Vehicle() {
			Registration = dto.Registration,
			Color = dto.Color,
			Year = dto.Year,
			VehicleModel = model
		};
		db.CreateVehicle(v);
		return Created($"/api/vehicles/{v.Registration}", v.ToResource());
	}

	//// PUT api/<VehiclesController>/5
	//[HttpPut("{id}")]
	//public void Put(int id, [FromBody] string value) {
	//}

	//// DELETE api/<VehiclesController>/5
	//[HttpDelete("{id}")]
	//public void Delete(int id) {
	//}
}
