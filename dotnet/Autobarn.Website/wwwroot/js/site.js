function showNotification(user, message) {
	var data = JSON.parse(message);
	console.log(data);
	var $target = $("div#signalr-notifications");
	var $html = $(`<div>New vehicle alert<br />` +
		`${data.Make} ${data.Model} (${data.Color}, ${data.Year})<br />` +
		`Price: ${data.Price} ${data.CurrencyCode}<br />` +
		`<a href="/vehicles/details/${data.Registration}">click for more info...</a>` +
		'</div>');
	$html.css("background-color", data.Color);
	$target.prepend($html);
	window.setTimeout(function () {
		$html.fadeOut(5000,
			function() {
				$html.remove();
			});
	}, 1000);

}

function connectToSignalR() {
	var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();

	conn.on("HeyShowThemAThing", showNotification);

	conn.start().then(function() {
		console.log("SignalR connected!");
	}).catch(function(err) {
		console.log("SignalR Failed!");
		console.log(err);
	});
}

$(connectToSignalR);
