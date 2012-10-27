using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLayer.Models;

namespace DbLayer.Context {
	public class AfricanCraftsContextSQL : DbContext {
		public AfricanCraftsContextSQL()
            : base("AfricanCraftsContextSQL")
        {
        }
		public DbSet<UserProfile> UserProfiles { get; set; }	
	}
}
