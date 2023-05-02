using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests; 

public class WebTests : IClassFixture<TestWebApplicationFactory<Startup>> {
	private readonly TestWebApplicationFactory<Startup> factory;

	public WebTests(TestWebApplicationFactory<Startup> factory) {
		this.factory = factory;
	}

	[Fact]
	public async void WebsiteWorks() {
		var client = factory.CreateClient();
		var response = await client.GetAsync("/");
		response.EnsureSuccessStatusCode();
	}
}

public class ApiTests : IClassFixture<TestWebApplicationFactory<Startup>> {
	private readonly TestWebApplicationFactory<Startup> factory;

	public ApiTests(TestWebApplicationFactory<Startup> factory) {
		this.factory = factory;
	}

	[Fact]
	public async Task GET_Vehicle_By_Id_Returns_Vehicle() {
		var http = factory.CreateClient();
		var result = await http.GetAsync("/api/vehicles/OUTATIME");
		var json = await result.Content.ReadAsStringAsync();
		var d = JsonConvert.DeserializeObject<JObject>(json);
		d["registration"].ShouldBe("OUTATIME");
		d["_links"].ShouldNotBeNull();
	}
}
