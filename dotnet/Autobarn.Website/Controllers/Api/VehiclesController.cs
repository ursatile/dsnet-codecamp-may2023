using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
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
	public IEnumerable<Vehicle> Get(int index = 0) {
		return db.ListVehicles().Skip(index).Take(PAGE_SIZE);
	}

	//// GET api/<VehiclesController>/5
	//[HttpGet("{id}")]
	//public string Get(int id) {
	//	return "value";
	//}

	//// POST api/<VehiclesController>
	//[HttpPost]
	//public void Post([FromBody] string value) {
	//}

	//// PUT api/<VehiclesController>/5
	//[HttpPut("{id}")]
	//public void Put(int id, [FromBody] string value) {
	//}

	//// DELETE api/<VehiclesController>/5
	//[HttpDelete("{id}")]
	//public void Delete(int id) {
	//}
}
