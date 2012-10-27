using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLayer.Models;

namespace DbLayer.Models {
	public class Product {
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public string UserName { get; set; }

		//public virtual UserProfile User { get; set; }

	}
}
