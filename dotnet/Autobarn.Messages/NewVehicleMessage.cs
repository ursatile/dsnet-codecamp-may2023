namespace Autobarn.Messages;

public class NewVehicleListed {
	public string Registration { get; set; }
	public string Make { get; set; }
	public string Model { get; set; }
	public string Color { get; set; }
	public int Year { get; set; }
	public DateTimeOffset ListedAt { get; set; }

	public override string ToString() {
		return $"{Registration} made for car {Make} {Model} with {Color} from {Year} in {ListedAt}";
	}
}
