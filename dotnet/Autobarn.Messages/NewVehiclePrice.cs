using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autobarn.Messages; 

public class NewVehiclePrice {
    public string Registration { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int Year { get; set; }
    public DateTimeOffset ListedAt { get; set; }
    public int Price { get; set; }
    public string CurrencyCode { get; set; }
}
