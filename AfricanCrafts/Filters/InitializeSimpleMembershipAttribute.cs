using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using DbLayer.Context;
using WebMatrix.WebData;

namespace AfricanCrafts.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
           LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
							Database.SetInitializer<AfricanCraftsContextSQL>(null);

                try
                {
									using (var context = new AfricanCraftsContextSQL())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
									//SQL
									WebSecurity.InitializeDatabaseConnection("AfricanCraftsContextSQL", "UserProfile", "UserId", "UserName", autoCreateTables: true);
									//Mysql 
									//WebSecurity.InitializeDatabaseConnection("AfricanCraftsContext", "userprofile", "UserId", "UserName", true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
