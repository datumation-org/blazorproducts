using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using datumation_products.Server.Data;
using datumation_products.Shared.Infrastructure.Caching;
using datumation_products.Shared.Infrastructure.Configuration;
using datumation_products.Shared.Infrastructure.Logging;
using datumation_products.Shared.Models;
using datumation_products.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace datumation_products.Server.Services {
    public interface IShoppingCartService {
        Task<ShoppingCartRemoveViewModel> AddToCart (int id);
        Task<int> AddToCart (Items item);
        Task<int> CartCount ();
        Task<List<Items>> CreateItemAsync (List<Items> itemList, ItemListType itemType);
        Task CreateNewItemAsync (Items itemNew, List<Items> theItems, ItemListType itemType);
        Task EmptyCart ();
        Task<int> GenerateCountAsync (Items itemNew);
        Task<List<Carts>> GetCartItems ();
        Task<int> GetCount ();
        Task<IEnumerable<Items>> getItems ();
        Task<IEnumerable<Result>> GetListResultAsync ();
        Task<IEnumerable<Result>> GetProviderDataByType ();
        Task<decimal> GetTotal ();
        Task MigrateCart (string userName);
        Task<ShoppingCartRemoveViewModel> RemoveFromCart (int id);
        Task<int> RemoveFromCartItem (int id);
        Task<ShoppingCartViewModel> ShoppingCart ();
    }

    public class ShoppingCartService : IShoppingCartService {
        public readonly DatumationProductsDbContext storeDB;

        //DatumationProductsDbContext storeDB = new DatumationProductsDbContext();
        public static string ShoppingCartId { get; set; } = Guid.NewGuid ().ToString ();
        private static ICacheProvider _cache;
        private readonly ICountService _countService;
        private readonly IDataRepo _repo;
        public ShoppingCartService (ICacheProvider cache, DatumationProductsDbContext context, ICountService countService, Data.IDataRepo repo) {
            _cache = cache;
            storeDB = context;
            _countService = countService;
            _repo = repo;
        }

        public static Task<ShoppingCartService> GetCart ([FromServices] DatumationProductsDbContext storeDB, [FromServices] ICountService countService, [FromServices] IDataRepo repo) {
            var cart = new ShoppingCartService (_cache, storeDB, countService, repo);
            return Task.FromResult (cart);
        }

        public async Task<IEnumerable<Result>> GetProviderDataByType () {
            return await _repo.ListProviders ();
        }

        // public async Task<List<Items>> GetTheRoutePath (RouteParam rt) {
        //     var itemList = await this.getItems ();
        //     var Items = new List<Items> ();

        //     if (rt.RouteType == RouteTypeEnm.SpecialtyIndex) {
        //         ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ("BEGIN PROJECT");
        //         Items = itemList.Where (a => a.CategoryId == 2).OrderBy (b => b.Specialty).DistinctBy (s => s.Specialty).ToList ();

        //         Items = await this.CreateItemAsync (Items,
        //             ItemListType.ProviderSpecialty);
        //     } else if (routeType.RouteType == RouteTypeEnm.StateIndex) {
        //         if (!string.IsNullOrEmpty (routeType.StateName)) {

        //             Items = itemList.Where (a => !string.IsNullOrEmpty (a.StateName))
        //                 .Where (a => a.StateName.Replace ("-", " ").ToUpper () == routeType.StateName.Replace ("-", " ").ToUpper () &&
        //                     !string.IsNullOrEmpty (a.Specialty))
        //                 .OrderBy (s => s.StateName)
        //                 .ToList ();
        //             Items = await this.CreateItemAsync (Items,
        //                 ItemListType.ProviderState);

        //         } else {

        //             Items = itemList.Where (a => a.CategoryId == 1)
        //                 .OrderBy (s => s.StateName)
        //                 .ToList ();
        //             Items = await this.CreateItemAsync (Items,
        //                 ItemListType.ProviderState);

        //         }
        //     } else if (routeType.RouteType == RouteTypeEnm.ComboIndex) {

        //         if (!string.IsNullOrEmpty (routeType.Specialty) && !string.IsNullOrEmpty (routeType.StateName)) {
        //             Items = itemList
        //                 .Where (a => a.CategoryId == 3 && a.Name.ToUpper ().IndexOf (routeType.RouteParamValue.ToUpper ()) != -1)
        //                 .DistinctBy (s => s.Name)
        //                 .OrderBy (s => s.Name)
        //                 .ToList ();
        //         } else {
        //             Items = itemList
        //                 .Where (a => a.CategoryId == 3)
        //                 .OrderBy (s => s.Name)
        //                 .ToList ();

        //         }
        //     }
        //     return await Task.FromResult (Items);
        // }

        private async Task<string> CreateItemPath (ItemListType itemType, Items item) {
            string thePath = "";
            thePath = item.RoutePath;
            return await Task.FromResult (thePath);
        }

        public async Task<List<Items>> CreateItemAsync (List<Items> itemList, ItemListType itemType) {
            List<Items> theItems = new List<Items> ();

            foreach (var itemNew in itemList) {
                await this.CreateNewItemAsync (itemNew, theItems, itemType);
            }
            return await Task.FromResult (theItems.ToList ());
        }

        public async Task<IEnumerable<Result>> GetListResultAsync () {
            return await _repo.ListProviders ();
        }

        public async Task<int> GenerateCountAsync (Items itemNew) {

            int recordCnt = 0;
            if (string.IsNullOrEmpty (itemNew.Specialty)) {
                itemNew.Specialty = "";
            }
            if (!string.IsNullOrEmpty (itemNew.Specialty) && string.IsNullOrEmpty (itemNew.StateName)) {
                itemNew.StateName = "";
                itemNew.RecordCount = await _countService.GetCountByTypeWithPar (itemNew.Specialty);
                recordCnt = itemNew.RecordCount;
            } else if (!string.IsNullOrEmpty (itemNew.StateAbbr) && string.IsNullOrEmpty (itemNew.Specialty)) {
                recordCnt = await _countService.GetCountByState (itemNew.StateAbbr);
            } else if (!string.IsNullOrEmpty (itemNew.StateAbbr) && !string.IsNullOrEmpty (itemNew.Specialty)) {
                recordCnt = itemNew.RecordCount;
                // recordCnt = await _countService.GetComboCount(itemNew.StateAbbr, itemNew.Specialty);
            }
            return recordCnt;
        }

        public async Task CreateNewItemAsync (Items itemNew, List<Items> theItems, ItemListType itemType) {
            int recCount = itemNew.RecordCount;
            //  int recCount = await GenerateCountAsync(itemNew);
            string routPath = await CreateItemPath (itemType, itemNew);
            var theItem = new Items {
                Carts = itemNew.Carts,
                Category = itemNew.Category,
                CategoryId = itemNew.CategoryId,
                Description = itemNew.Description,
                Id = itemNew.Id,
                InternalImage = itemNew.InternalImage,
                ItemPictureUrl = itemNew.ItemPictureUrl,
                Name = itemNew.Name,
                OrderDetails = itemNew.OrderDetails,
                Price = itemNew.Price,
                RecordCount = recCount,
                RoutePath = routPath,
                Specialty = itemNew.Specialty,
                StateName = itemNew.StateName,
                ColorScheme = itemNew.ColorScheme
            };

            theItems.Add (theItem);
        }
        public async Task<ShoppingCartViewModel> ShoppingCart () {
            var cart = await GetCart (storeDB, _countService, _repo);

            //Set up our ViewModel
            var viewModel = new ShoppingCartViewModel {
                CartItems = await cart.GetCartItems (),
                CartTotal = await cart.GetTotal ()
            };
            //Return the view
            return viewModel;
        }

        public async Task<ShoppingCartRemoveViewModel> AddToCart (int id) {
            // Retrieve the item from the database
            var addedItem = await storeDB.Items
                .SingleAsync (item => item.Id == id);

            // Add it to the shopping cart

            int count = 0;
            var cart = await GetCart (storeDB, _countService, _repo);
            var currentItems = await cart.GetCartItems ();
            if (!currentItems.Any (s => s.Item.Id == id)) {
                count = await cart.AddToCart (addedItem);
            }

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel {
                Message = HttpUtility.HtmlEncode (addedItem.Name) +
                " has been added to your shopping cart.",
                CartTotal = await cart.GetTotal (),
                CartCount = await cart.GetCount (),
                ItemCount = count,
                DeleteId = id
            };
            return results;

        }

        public async Task<ShoppingCartRemoveViewModel> RemoveFromCart (int id) {
            // Remove the item from the cart
            var cart = await GetCart (storeDB, _countService, _repo);

            // Get the name of the item to display confirmation

            // Get the name of the album to display confirmation
            string itemName = storeDB.Items
                .Single (item => item.Id == id).Name;

            // Remove from cart
            int itemCount = await cart.RemoveFromCartItem (id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel {
                Message = "One (1) " + HttpUtility.HtmlEncode (itemName) +
                " has been removed from your shopping cart.",
                CartTotal = await cart.GetTotal (),
                CartCount = await cart.GetCount (),
                ItemCount = itemCount,
                DeleteId = id
            };

            return results;
        }

        public async Task<int> CartCount () {
            var cart = await GetCart (storeDB, _countService, _repo);
            return await cart.GetCount ();
        }

        public async Task<int> AddToCart (Items item) {
            // Get the matching cart and item instances
            var cartItem = await storeDB.Carts.SingleOrDefaultAsync (
                c => c.CartId == ShoppingCartId &&
                c.ItemId == item.Id);

            if (cartItem == null) {
                // Create a new cart item if no cart item exists
                cartItem = new Carts {
                ItemId = item.Id,
                CartId = ShoppingCartId,
                Count = 1,
                DateCreated = DateTime.Now
                };
                await storeDB.Carts.AddAsync (cartItem);
            } else {
                // If the item does exist in the cart,
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            await storeDB.SaveChangesAsync ();

            return cartItem.Count;
        }

        public async Task<int> RemoveFromCartItem (int id) {
            // Get the cart

            var cartItem = await storeDB.Carts.SingleAsync (
                cart => cart.CartId == ShoppingCartId &&
                cart.ItemId == id);

            int itemCount = 0;

            if (cartItem != null) {
                if (cartItem.Count > 1) {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                } else {
                    storeDB.Carts.Remove (cartItem);
                }
                // Save changes
                await storeDB.SaveChangesAsync ();
            }
            return itemCount;
        }

        public async Task EmptyCart () {
            var cartItems = storeDB.Carts.Where (
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems) {
                storeDB.Carts.Remove (cartItem);
            }
            // Save changes
            _ = await storeDB.SaveChangesAsync ();
        }

        public Task<List<Carts>> GetCartItems () {
            var cartItems = storeDB.Carts.Include ("Item").Where (
                cart => cart.CartId == ShoppingCartId).ToListAsync ();
            return cartItems;
        }

        public async Task<IEnumerable<Items>> getItems () {
            string cacheKey = "CACHE_KEY_GET_ITEMS";
            IEnumerable<Items> items = new List<Items> ();
            try {
                items = _cache.Retrieve<List<Items>> (cacheKey);
            } catch (System.Exception ex) {
                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    SHOPPING CART SERVICE [getItems]: ERROR --==--==> {ex.Message}
                ");
            }
            if (items == null) {

                items = await _repo.AllItems ();

                if (items.Any ()) {
                    _cache.Store (cacheKey, items);
                }
            }
            return items;

        }

        public async Task<int> GetCount () {
            // Get the count of each item in the cart and sum them up
            int? count = await (from cartItems in storeDB.Carts where cartItems.CartId == ShoppingCartId select (int?) cartItems.Count).SumAsync ();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public async Task<decimal> GetTotal () {
            // Multiply item price by count of that item to get
            // the current price for each of those items in the cart
            // sum all item price totals to get the cart total
            decimal? total = await (from cartItems in storeDB.Carts where cartItems.CartId == ShoppingCartId select (int?) cartItems.Count *
                cartItems.Item.Price).SumAsync ();

            return total ?? decimal.Zero;
        }

        // public async Task<Orders> CreateOrder(Orders order)
        // {
        //     decimal orderTotal = 0;
        //     order.OrderDetails = new List<OrderDetails>();

        //     var cartItems = await GetCartItems();
        //     // Iterate over the items in the cart,
        //     // adding the order details for each
        //     foreach (var item in cartItems)
        //     {
        //         var orderDetail = new OrderDetails
        //         {
        //             ItemId = item.ItemId,
        //             OrderId = order.OrderId,
        //             UnitPrice = item.Item.Price,
        //             Quantity = item.Count
        //         };
        //         // Set the order total of the shopping cart
        //         orderTotal += (item.Count * item.Item.Price);
        //         order.OrderDetails.Add(orderDetail);
        //         storeDB.OrderDetails.Add(orderDetail);

        //     }
        //     // Set the order's total to the orderTotal count
        //     order.Total = orderTotal;

        //     // Save the order
        //     await storeDB.SaveChangesAsync();
        //     // Empty the shopping cart
        //     await EmptyCart();
        //     // Return the OrderId as the confirmation number
        //     return order;
        // }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public async Task MigrateCart (string userName) {
            var shoppingCart = storeDB.Carts.Where (
                c => c.CartId == ShoppingCartId);

            foreach (Carts item in shoppingCart) {
                item.CartId = userName;
            }
            _ = await storeDB.SaveChangesAsync ();
        }
    }
}