using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DbLayer.Models;
using DbLayer.Context;
using System.Threading.Tasks;

namespace AfricanCraftsApi.Controllers
{
    public class ProductController : ApiController
    {
        private AfricanCraftsContext db = new AfricanCraftsContext();

        // GET Product
				public async Task<IEnumerable<Product>> GetProducts()
        {
					return Product.GetProducts();
					//using (var context = new AfricanCraftsContext()) {
					//	return await from s in context.Product
					//								orderby s.Name
					//								select s;
					//}
					//return db.Product.All;
					//db.Product.AsEnumerable();
						//using (var db2 = new AfricanCraftsContext()) {
							// Display all Blogs from the database
					//var products = from b in db.Product
					//							 orderby b.Name
					//							 select b;
					//System.Threading.Thread.Sleep(5000);
					//return products;
						//	System.Threading.Thread.Sleep(5000);
					//return AllProducts(); //db.Product.AsEnumerable;

					//	}
        }

				public IEnumerable<Product> AllProducts() {
					using (var db2 = new AfricanCraftsContext()) {
						var products = (from b in db2.Product
													 orderby b.Name
													 select b).ToList();
						return products;
					
						//yield return new Product();
					}
				}

        // GET api/Product/5
        public Product GetProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return product;
        }

        // PUT api/Product/5
        public HttpResponseMessage PutProduct(int id, Product product)
        {
            if (ModelState.IsValid && id == product.ProductId)
            {
                db.Entry(product).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Product
        public HttpResponseMessage PostProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, product);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = product.ProductId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Product/5
        public HttpResponseMessage DeleteProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Product.Remove(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}