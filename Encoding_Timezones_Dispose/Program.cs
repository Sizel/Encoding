using System;
using System.IO;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace Encoding_Timezones_Dispose
{
	class Program
	{
		static void Main(string[] args)
		{
			TimeZoneInfo brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");

			DateTimeOffset moldovaTime = new DateTime(2018, 2, 20, 12, 30, 0);
			DateTimeOffset brasilTime = TimeZoneInfo.ConvertTime(moldovaTime, brasilTimeZone);

			Order moldovaOrder = new Order( "Candy shop", "Moldova", moldovaTime, 3000.5f);
			Order brasilOrder = new Order("Another candy shop", "Brasil", brasilTime, 1700.5f);

			var orders = new List<Order>() { moldovaOrder, brasilOrder };

			Console.WriteLine("==== Orders ====");
			PrintCollection(orders);

			#region Use of encodings
			FileStream fileStream = new FileStream(@".\out.txt", FileMode.OpenOrCreate, FileAccess.Write);
			
			byte[] encodedCountryName = Encoding.UTF8.GetBytes(moldovaOrder.CountryName);

			fileStream.Write(encodedCountryName, 0, encodedCountryName.Length);
			fileStream.Close();

			fileStream = new FileStream(@".\out.txt", FileMode.Open, FileAccess.Read);

			byte[] decodedCountryName = new byte[fileStream.Length];
			fileStream.Read(decodedCountryName, 0, (int)fileStream.Length);

			string readName = Encoding.UTF8.GetString(decodedCountryName);
			//string readName = Encoding.UTF32.GetString(buffer); doesn't work

			Console.WriteLine(readName);

			#endregion

			#region String search

			Console.WriteLine("==== Count how many candy shops ====");

			int counter = 0;

			foreach (var order in orders)
			{
				if (order.ShopName.ToLower().Contains("candy"))
				{
					counter++;
				}
			}

			Console.WriteLine($"There are { counter } candy shops out there");
			Console.WriteLine();
			#endregion

			#region String comparison
			Console.WriteLine("==== Count how many orders from Moldova ====");

			counter = 0;

			foreach (var order in orders)
			{
				if (order.CountryName.ToLower() == "Moldova")
				{
					counter++;
				}
			}

			Console.WriteLine($"№ of orders from Moldova = { counter }");
			Console.WriteLine();
			#endregion

			#region String formatting and using CultureInfo
			CultureInfo us = CultureInfo.GetCultureInfo("en-US");
			for (int i = 0; i < orders.Count; i++)
			{
				Console.WriteLine($"Order { i }: { orders[i].Date.Value.ToString("D") } total: { orders[i].Total.ToString("C", us) }");
			}
			#endregion

			#region Use of TimeSpans 
			Console.WriteLine($"Years since the order was made: { GetYearsSince(moldovaOrder) }");
			#endregion

			#region Use of DateTimeOffsets
			Console.WriteLine(WasMadeInTheSameHour(moldovaOrder, brasilOrder));
			#endregion


		}

		static int GetYearsSince(Order order)
		{
			if (order.Date.HasValue)
			{
				TimeSpan timeElapsed = DateTime.Now - order.Date.Value;
				int years = (int)(timeElapsed.TotalDays / 365f);
				return years;
			}
			else
			{
				throw new ArgumentNullException("date");
			}
		}

		static bool WasMadeInTheSameHour(Order order1, Order order2)
		{
			if (!order1.Date.HasValue)
			{
				throw new ArgumentNullException("order1");
			}
			else if (!order2.Date.HasValue)
			{
				throw new ArgumentNullException("order2");
			}
			else
			{
				return	(order1.Date.Value.Year == order2.Date.Value.Year)   &&
						(order1.Date.Value.Month == order2.Date.Value.Month) &&
						(order1.Date.Value.Day == order2.Date.Value.Day)     &&
						(order1.Date.Value.Hour == order2.Date.Value.Hour);
			}
		}


		static void PrintCollection(IEnumerable c)
		{
			foreach (var item in c)
			{
				Console.WriteLine($"{ item } ");
			}
			Console.WriteLine();
		}

	}
}
