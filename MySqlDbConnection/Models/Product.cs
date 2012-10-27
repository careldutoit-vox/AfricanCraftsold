using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLayer.Models;

namespace DbLayer.Models {
	public class Product {
		public Product() { }   // empty ctor required
		public Product(int cnt) {
			ProductId = cnt;
			Name = "Product " + cnt.ToString();
			Description = "product";
		}
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public string UserName { get; set; }

		// Simulate a call to a back end store such as SQL Server
		public static IEnumerable<Product> GetProducts() {
			for (int i = 1; i < 1000; i++) {
				yield return new Product(i);
			}
		}
	}
}
