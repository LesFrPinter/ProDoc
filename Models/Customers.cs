using System;

namespace ProDocEstimate.ViewModels
{
	public class Customers
	{
		public Guid ID { get; set; }
		public string? CustomerName { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? ZIP { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
	}
}