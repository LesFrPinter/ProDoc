using System;

namespace ProDocEstimate.ViewModels
{
	public class Customers
	{
		public Guid ID { get; set; }
		public string? CUST_NUMB { get; set; }
		public string? CUST_NAME { get; set; }
		public string? ADDRESS1 { get; set; }
		public string? ADDRESS2 { get; set; }
		public string? CITY { get; set; }
		public string? STATE { get; set; }
		public string? ZIP { get; set; }
		public string? ATTENTION { get; set; }
		public string? PHONE { get; set; }
	}
}