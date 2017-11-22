using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Memberships.Entities;
using Memberships.Models;
using Memberships.Areas.Admin.Models;
using Memberships.Extensions;

namespace Memberships.Areas.Admin.Controllers
{
    public class ProductItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductItem
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductItems.Convert(db));
        }

        // GET: Admin/ProductItem/Details/5
        public async Task<ActionResult> Details(int? itemId, int? productid)
        {
            if (itemId == null || productid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productid);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.Convert(db));
        }

        // GET: Admin/ProductItem/Create
        public async Task<ActionResult> Create()
        {
            var model = new ProductItemModel
            {
                Items = await db.Items.ToListAsync(),
                Products = await db.Products.ToListAsync()
            };
            return View(model);
        }

        // POST: Admin/ProductItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,ItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                db.ProductItems.Add(productItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productItem);
        }

        // GET: Admin/ProductItem/Edit/5
        public async Task<ActionResult> Edit(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            var productItemModel = await productItem.Convert(db);
            return View(productItemModel);
        }

        // POST: Admin/ProductItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "ProductId,ItemId,OldProductId,OldItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                if (await productItem.CanChange(db))
                    await productItem.Change(db);
                
                //db.Entry(productItem).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productItem);
        }

        // GET: Admin/ProductItem/Delete/5
        public async Task<ActionResult> Delete(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            return View(await productItem.Convert(db));
        }

        // POST: Admin/ProductItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int itemId, int productId)
        {
            ProductItem productItem = await GetProductItem(itemId, productId);
            db.ProductItems.Remove(productItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<ProductItem> GetProductItem(int? ItemId, int? ProductId )
        {
            try
            {
                int itmId = 0, productId = 0;
                int.TryParse(ItemId.ToString(), out itmId);
                int.TryParse(ProductId.ToString(), out productId);
                var productItem = await db.ProductItems.FirstOrDefaultAsync(
                    pi => pi.ProductId.Equals(productId) && pi.ItemId.Equals(itmId));
                return productItem;

            }
            catch { return null; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
