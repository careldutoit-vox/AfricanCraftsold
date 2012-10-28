using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DbLayer.Context;
using DbLayer.Models;
using Newtonsoft.Json;

namespace AfricanCrafts.Controllers {
	public class ProductController : Controller {
		//
		// GET: /Product/

		public ActionResult Index() {
			ViewBag.SyncOrAsync = "Synchronous";
			List<Product> productList = new List<Product>();
			using (var db = new AfricanCraftsContext()) {
				// Display all Blogs from the database
				var products = from b in db.Product
											 orderby b.Name
											 select b;
				productList.AddRange(products);
				

			}
			return View(productList);
		}
		[AsyncTimeout(150)]
		[HandleError(ExceptionType = typeof(TimeoutException),
																				View = "TimeoutError")]
		public async Task<ActionResult> ProductAsync() {
			ViewBag.SyncOrAsync = "Asynchronous";
			return View("Product", await GetProductAsync());
			
		}

		public async Task<List<Product>> GetProductAsync() {
			
			using (HttpClient client = new HttpClient()) {
				HttpResponseMessage response = await client.GetAsync("http://localhost:8333/product");
				return await response.Content.ReadAsAsync<List<Product>>();
			
			}
		}

		// GET: /Product/Details/5

		public ActionResult Details(int id) {
			return View();
		}

		//
		// GET: /Product/Create
		[Authorize]
		public ActionResult Create() {
			var user = User.Identity.Name;
			return View();
		}

		//
		// POST: /Product/Create

		[HttpPost]
		public ActionResult Create(Product productCol) {
			try {

				using (var db = new AfricanCraftsContext()) {
					var product = new Product {
						Name = productCol.Name,
						Description = productCol.Description,
						UserName = User.Identity.Name
					};
					db.Product.Add(product);
					db.SaveChanges();
				}

				return RedirectToAction("Index");
			} catch {
				return View();
			}
		}

		//
		// GET: /Product/Edit/5

		public ActionResult Edit(int id) {
			return View();
		}

		//
		// POST: /Product/Edit/5

		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			try {
				// TODO: Add update logic here

				return RedirectToAction("Index");
			} catch {
				return View();
			}
		}

		//
		// GET: /Product/Delete/5

		public ActionResult Delete(int id) {
			return View();
		}

		//
		// POST: /Product/Delete/5

		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection) {
			try {
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			} catch {
				return View();
			}
		}
	}
}
