using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Memberships.Extensions
{
    public static class ConversionExtentions
    {
        #region ProductItemModel 
        // Converts a List of Product Items into a proper list of Product Item Models 
        public static async Task<IEnumerable<ProductItemModel>> Convert(
        this IQueryable<ProductItem> productItems,
        ApplicationDbContext db)
        {
            if (productItems.Count().Equals(0))
                return new List<ProductItemModel>();
            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return await (from pi in productItems
                   select new ProductItemModel
                   {
                        ProductId = pi.ProductId,
                        ItemId = pi.ItemId,
                        ItemTitle = db.Items.FirstOrDefault(
                            i => i.Id.Equals(pi.ItemId)).Title,
                       ProductTitle = db.Products.FirstOrDefault(
                            p => p.Id.Equals(pi.ProductId)).Title,

                   }).ToListAsync();

        }
        //Converts a Product Item into a Proper Product Item Model
        public static async Task<ProductItemModel> Convert(
            this ProductItem product,
            ApplicationDbContext db,
            bool addListData = true)
        {


            var model = new ProductItemModel
            {
                ProductId = product.ProductId,
                ItemId = product.ItemId,
                Items = addListData ? await db.Items.ToListAsync() : null,
                Products = addListData ?  await db.Products.ToListAsync() : null,
                ItemTitle = (await db.Items.FirstOrDefaultAsync(
                    i => i.Id.Equals(product.ItemId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(
                    p => p.Id.Equals(product.ProductId))).Title,
            };


            return model;

        }
        public static async Task<bool> CanChange(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldPi = await db.ProductItems.CountAsync(
                pi => pi.ProductId.Equals(productItem.OldProductId) &&
                pi.ItemId.Equals(productItem.OldItemId));

            var newPi = await db.ProductItems.CountAsync(
                pi => pi.ProductId.Equals(productItem.ProductId) &&
                pi.ItemId.Equals(productItem.ItemId));

            return oldPi.Equals(1) && newPi.Equals(0);

        }

        public static async Task Change(this ProductItem productItem, ApplicationDbContext db)
        {
            var oldProductItem = await db.ProductItems.FirstOrDefaultAsync(
                pi => pi.ProductId.Equals(productItem.OldProductId) &&
                pi.ItemId.Equals(productItem.OldItemId));
            var newProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(
                productItem.ProductId) &&
                pi.ItemId.Equals(productItem.ItemId));

            if (oldProductItem != null && newProductItem == null)
            {
                newProductItem = new ProductItem
                {
                    ItemId = productItem.ItemId,
                    ProductId = productItem.ProductId
                };

                using (var transaction = new System.Transactions.TransactionScope(
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.ProductItems.Remove(oldProductItem);
                        db.ProductItems.Add(newProductItem);

                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }

            }

        }
        #endregion
        #region ProductModel
        // Converts a List of Product into a proper list of Product Models 
        public static async Task<IEnumerable<ProductModel>>  Convert(
            this IEnumerable<Product> products,
            ApplicationDbContext db )
        {
            if (products.Count().Equals(0))
                return new List<ProductModel>();
            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return from p in products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductTypeId = p.ProductTypeId,
                       ProductLinkTexts = texts,
                       ProductTypes = types
                   };

        }
        // Converts a Product into a proper Product Model 
        public static async Task<ProductModel> Convert(
            this Product product,
            ApplicationDbContext db)
        {

            var text = await db.ProductLinkTexts.FirstOrDefaultAsync(
                p => p.Id.Equals(product.ProductLinkTextId));
            var type = await db.ProductTypes.FirstOrDefaultAsync(
                p => p.Id.Equals(product.ProductTypeId));

            var model = new ProductModel
                   {
                       Id = product.Id,
                       Title = product.Title,
                       Description = product.Description,
                       ImageUrl = product.ImageUrl,
                       ProductLinkTextId = product.ProductLinkTextId,
                       ProductTypeId = product.ProductTypeId,
                       ProductLinkTexts = new List<ProductLinkText>(),
                       ProductTypes = new List<ProductType>()
            };
            model.ProductLinkTexts.Add(text);
            model.ProductTypes.Add(type);

            return model;

        }
        #endregion

        #region SubscriptionProductModel
        // Converts a List of Product Items into a proper list of Product Item Models 
        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(
        this IQueryable<SubscriptionProduct> subscriptionProducts,
        ApplicationDbContext db)
        {
            if (subscriptionProducts.Count().Equals(0))
                return new List<SubscriptionProductModel>();
            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return await (from sp in subscriptionProducts
                          select new SubscriptionProductModel
                          {
                              ProductId = sp.ProductId,
                              SubscriptionId = sp.SubscriptionId,
                              SubscriptionTitle = db.Subscriptions.FirstOrDefault(
                                   i => i.Id.Equals(sp.SubscriptionId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(
                                   p => p.Id.Equals(sp.ProductId)).Title,

                          }).ToListAsync();

        }
        //Converts a Product Item into a Proper Product Item Model
        public static async Task<SubscriptionProductModel> Convert(
            this SubscriptionProduct subscriptionProduct,
            ApplicationDbContext db,
            bool addListData = true)
        {


            var model = new SubscriptionProductModel
            {
                ProductId = subscriptionProduct.ProductId,
                SubscriptionId = subscriptionProduct.SubscriptionId,
                Subscriptions = addListData ? await db.Subscriptions.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                SubscriptionTitle = (await db.Subscriptions.FirstOrDefaultAsync(
                    i => i.Id.Equals(subscriptionProduct.SubscriptionId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(
                    p => p.Id.Equals(subscriptionProduct.ProductId))).Title,
            };


            return model;

        }
        public static async Task<bool> CanChange(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldSp = await db.SubscriptionProducts.CountAsync(
                sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSp = await db.SubscriptionProducts.CountAsync(
                sp => sp.ProductId.Equals(subscriptionProduct.ProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            return oldSp.Equals(1) && newSp.Equals(0);

        }

        public static async Task Change(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var oldSp = await db.SubscriptionProducts.FirstOrDefaultAsync(
                sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var newSp = await db.SubscriptionProducts.FirstOrDefaultAsync(
                sp => sp.ProductId.Equals(subscriptionProduct.ProductId) &&
                sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            if (oldSp != null && newSp == null)
            {
                newSp = new SubscriptionProduct
                {
                    SubscriptionId = subscriptionProduct.SubscriptionId,
                    ProductId = subscriptionProduct.ProductId
                };

                using (var transaction = new System.Transactions.TransactionScope(
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        db.SubscriptionProducts.Remove(oldSp);
                        db.SubscriptionProducts.Add(newSp);

                        await db.SaveChangesAsync();
                        transaction.Complete();
                    }
                    catch { transaction.Dispose(); }
                }

            }

        }
        #endregion

    }
}