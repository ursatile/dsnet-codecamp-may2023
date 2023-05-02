using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data.Entities;

public static class HypermediaExtensions {
	public static dynamic ToDynamic(this object value) {
		IDictionary<string, object> expando = new ExpandoObject();
		var properties = TypeDescriptor.GetProperties(value.GetType());
		foreach (PropertyDescriptor property in properties) {
			if (Ignore(property)) continue;
			expando.Add(property.Name, property.GetValue(value));
		}
		return (ExpandoObject) expando;
	}

	public static dynamic ToResource(this Vehicle vehicle) {
		var result = vehicle.ToDynamic();
		result._links = new {
			self = new {
				href = $"/api/vehicles/{vehicle.Registration}"
			}
		};
		return result;
	}

	private static bool Ignore(PropertyDescriptor property)
		=> property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();
}
