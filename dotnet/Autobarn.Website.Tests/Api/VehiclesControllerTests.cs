using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.Api;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Shouldly;

namespace Autobarn.Website.Tests.Api {
	public class VehiclesControllerTests {

		[Fact]
		public void ExampleTest() {
			var foo = "this";
			foo.Length.ShouldBe(4);
		}

		[Fact]
		public void GET_Vehicle_By_Id_Returns_Vehicle() {
			var db = new FakeDb();
			var bus = new FakeBus();
			var c = new VehiclesController(db, bus);
			var id = "TESTCAR1";
			var result = c.Get(id) as OkObjectResult;
			result.ShouldNotBeNull();
			dynamic d = result.Value as dynamic;
			((string)d.Registration).ShouldBe(id);
		}
	}

	public class FakeBus : IBus {
		public void Dispose() {
			throw new System.NotImplementedException();
		}

		public IPubSub PubSub { get; }
		public IRpc Rpc { get; }
		public ISendReceive SendReceive { get; }
		public IScheduler Scheduler { get; }
		public IAdvancedBus Advanced { get; }
	}

	public class FakeDb : IAutobarnDatabase {
		public int CountVehicles() {
			throw new System.NotImplementedException();
		}

		public IEnumerable<Vehicle> ListVehicles() {
			throw new System.NotImplementedException();
		}

		public IEnumerable<Manufacturer> ListManufacturers() {
			throw new System.NotImplementedException();
		}

		public IEnumerable<Model> ListModels() {
			throw new System.NotImplementedException();
		}

		public Vehicle FindVehicle(string registration) {
			return new Vehicle {Registration = registration};
		}

		public Model FindModel(string code) {
			throw new System.NotImplementedException();
		}

		public Manufacturer FindManufacturer(string code) {
			throw new System.NotImplementedException();
		}

		public void CreateVehicle(Vehicle vehicle) {
			throw new System.NotImplementedException();
		}

		public void UpdateVehicle(Vehicle vehicle) {
			throw new System.NotImplementedException();
		}

		public void DeleteVehicle(Vehicle vehicle) {
			throw new System.NotImplementedException();
		}
	}
}
