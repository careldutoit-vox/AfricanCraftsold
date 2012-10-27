using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLayer.Models;

namespace DbLayer.Context {
	public class AfricanCraftsContext : DbContext {
		public AfricanCraftsContext()
            : base("AfricanCraftsContext")
        {
        }
		public DbSet<Product> Product { get; set; }	
	}
}
