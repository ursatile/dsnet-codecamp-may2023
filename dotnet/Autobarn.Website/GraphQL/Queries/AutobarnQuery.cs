using System;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries;

public class AutobarnQuery : ObjectGraphType {
	private readonly IAutobarnDatabase db;

	public AutobarnQuery(IAutobarnDatabase db) {
		this.db = db;
		Field<ListGraphType<ManufacturerGraphType>>("Manufacturers")
			.Description("Return all manufacturers")
			.Resolve(GetAllManufacturers);

		Field<ListGraphType<VehicleGraphType>>("Vehicles")
			.Description("Return all vehicles")
			.Resolve(GetAllVehicles);

		Field<VehicleGraphType>("Vehicle")
			.Description("Return a single vehicle")
			.Arguments(MakeNonNullStringArgument("registration", "The vehicle registration plate"))
			.Resolve(GetVehicle);

		Field<ListGraphType<VehicleGraphType>>("VehiclesByColor")
			.Description("Query to retrieve all Vehicles matching the specified color")
			.Arguments(MakeNonNullStringArgument("color", "The name of a color, eg 'blue', 'grey'"))
			.Resolve(GetVehiclesByColor);
	}

	private QueryArgument MakeNonNullStringArgument(string name,
		string description) {
		return new QueryArgument<NonNullGraphType<StringGraphType>> {
			Name = name, Description = description
		};
	}

	private Vehicle GetVehicle(IResolveFieldContext context) {
		var registration = context.GetArgument<string>("registration");
		return db.FindVehicle(registration);
	}

	private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext context)
		=> db.ListVehicles();

	private IEnumerable<Manufacturer> GetAllManufacturers(IResolveFieldContext context)
		=> db.ListManufacturers();

	private IEnumerable<Vehicle>
		GetVehiclesByColor(IResolveFieldContext context) {
		var color = context.GetArgument<string>("color");
		if (color == "error") {
			context.Errors.Add(new ExecutionError("Sorry, 'error' is not a valid color"));
			return null;
		}
		var vehicles = db.ListVehicles().Where(v => v.Color.Contains(color));
		return vehicles;
	}
}
