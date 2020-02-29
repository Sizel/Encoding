using System;
using System.Collections.Generic;
using System.Text;

namespace Encoding_Timezones_Dispose
{
	class Order
	{
		public string ShopName { get; set; }
		public string CountryName { get; set; }

		public DateTimeOffset? Date { get; set; }
		
		public double Total { get; set; }
		
		public Order(string shopName, string countryName, DateTimeOffset? date, double total)
		{
			ShopName = shopName;
			CountryName = countryName;
			Date = date;
			Total = total;
		}

		public override string ToString()
		{
			return $"Shop name: { ShopName, 20 }, Country: { CountryName, 15 }, Date: { Date.Value.ToString("D"), 10} Total: { Total, 5 }";
		}
	}
}
