using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using FYPJ_Web_App_Insecure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FYPJ_Web_App_Insecure.Controllers
{
    public class CartController : Controller
    {

        private readonly databaseContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController(databaseContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        private Products GetProductById(string id)
        {

            var product = _db.Products.FromSqlRaw($"SELECT * FROM products WHERE ProductId='{id}'");
            return product.FirstOrDefault();
        }

        private CartItem GetProductsFromCart(int? userid)
        {

            var cart = _db.Cart.Where(x => x.UserId.Equals(userid)).FirstOrDefault();
            var cartItems = _db.CartItem.FromSqlRaw($"Select * from cartitem where shoppingcartid = '{cart.CartId}' ").FirstOrDefault();
            return cartItems;
        }








        public IActionResult Cart()
        {

            var currentUserId = HttpContext.Session.GetString("UserId");

            if (currentUserId == null)
            {
                ViewBag.Notification = "Login to add things to cart!";
                ViewBag.cart = null;
                return View();
            }
            else
            {
                ViewBag.products = _db.Products.ToList();
                var product = _db.Products.ToList();
                var cart = _db.Cart.Where(x => x.UserId.Equals(currentUserId)).FirstOrDefault();
                var cartItems = _db.CartItem.Where(x => x.ShoppingCartId.Equals(cart.CartId)).ToList();
                //var cartItems = _db.CartItem.ToList();
                if (cartItems.Count > 0)
                {

                    ViewBag.cart = 1;
                    var subtotal = 0;
                    foreach (var x in cartItems)
                    {

                        subtotal += x.Quantity * (int)GetProductById($"{x.ProductId}").Price;
                    }
                    var cartUser = new CartUser() { Cart = cart, CartItem = cartItems, Products = product, SubTotal = (short)subtotal };
                    return View(cartUser);
                }
                else
                {
                    ViewBag.cart = null;

                }
                return View();

            }
        }

        [HttpGet("Cart/AddToCart")]
        [HttpGet("Cart/AddToCart/{id:int}")]
        public IActionResult AddToCart(int id)
        {
            if (HttpContext.Request.Cookies["user_id"] != null)
            {
                var currentUserId = Int32.Parse(HttpContext.Request.Cookies["user_id"]);
                var cart = _db.Cart.Where(x => x.UserId.Equals(currentUserId)).FirstOrDefault();
                if (cart == null)
                {
                    var cartEntry = new Cart { UserId = currentUserId };
                    _db.Add(cartEntry);
                    _db.SaveChanges();

                    var cartNew = _db.Cart.Where(x => x.UserId.Equals(currentUserId)).FirstOrDefault();
                    var cartItemNew = _db.CartItem.Where(x => x.ProductId.Equals(id) && x.ShoppingCartId.Equals(cartEntry.CartId)).FirstOrDefault();
                    var items = GetProductById($"{id}");
                    if (cartItemNew == null)
                    {
                        var cartItemEntry = new CartItem { ProductId = id, Quantity = 1, ShoppingCartId = cartNew.CartId, ProductPrice = items.Price, ProductTitle = items.Title };
                        _db.Add(cartItemEntry);
                        _db.SaveChanges();
                    }
                    else
                    {
                        _db.CartItem.Update(new CartItem { Id = cartItemNew.Id, ProductId = cartItemNew.ProductId, Quantity = cartItemNew.Quantity + 1, ShoppingCartId = cartItemNew.ShoppingCartId, ProductPrice = cartItemNew.ProductPrice, ProductTitle = cartItemNew.ProductTitle });
                        _db.SaveChanges();
                    }

                }
                else
                {
                    var cartEntry = _db.Cart.Where(x => x.UserId.Equals(currentUserId)).FirstOrDefault();
                    var cartItemNew = _db.CartItem.Where(x => x.ProductId.Equals(id) && x.ShoppingCartId.Equals(cartEntry.CartId)).FirstOrDefault();
                    var items = GetProductById($"{id}");

                    if (cartItemNew == null)
                    {
                        var cartItemEntry = new CartItem
                        {
                            ProductId = id,
                            Quantity = 1,
                            ShoppingCartId = cartEntry.CartId,
                            ProductPrice = items.Price,
                            ProductTitle = items.Title
                        };
                        _db.Add(cartItemEntry);
                        _db.SaveChanges();
                    }
                    else
                    {

                        //_db.CartItem.Update(new CartItem { Id = cartItemNew.Id, ProductId = cartItemNew.ProductId, Quantity = cartItemNew.Quantity + 1, ShoppingCartId = cartItemNew.ShoppingCartId });
                        cartItemNew.Quantity += 1;
                        _db.SaveChanges();
                    }
                }



                return RedirectToAction("Index", "Shop");

            }
            else
            {
                TempData["ErrorMessage"] = "Please login to add to cart!";
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpGet("Cart/AddQuantity")]
        [HttpGet("Cart/AddQuantity/{productId:int}")]
        public IActionResult AddQuantity(int productId)
        {
            var cItem = _db.CartItem.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            if (cItem != null)
            {
                cItem.Quantity += 1;
                _db.SaveChanges();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("cItem null");
            }

            return RedirectToAction("Cart", "Cart");
        }

        [HttpGet("Cart/RemoveQuantity")]
        [HttpGet("Cart/RemoveQuantity/{productId:int}")]
        public IActionResult RemoveQuantity(int productId)
        {
            var cItem = _db.CartItem.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            if (cItem != null)
            {
                cItem.Quantity -= 1;
                _db.SaveChanges();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("cItem null");
            }
            return RedirectToAction("Cart", "Cart");
        }

        [HttpGet("Cart/RemoveOrder")]
        [HttpGet("Cart/RemoveOrder/{productId:int}")]
        public IActionResult RemoveOrder(int productId)
        {
            var cItem = _db.CartItem.Where(x => x.Id.Equals(productId)).FirstOrDefault();
            if (cItem != null)
            {
                _db.CartItem.Remove(cItem);
                _db.SaveChanges();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("cItem null");
            }
            return RedirectToAction("Cart", "Cart");
        }

        public IActionResult Checkout()
        {

            var currentUserId = HttpContext.Session.GetString("UserId");

            if (currentUserId != null)
            {
                var product = _db.Products.ToList();
                var cart = _db.Cart.Where(x => x.UserId.Equals(currentUserId)).FirstOrDefault();
                var cartItems = _db.CartItem.Where(x => x.ShoppingCartId.Equals(cart.CartId)).ToList();
                if (cartItems.Count > 0)
                {

                    ViewBag.cart = 1;
                    var subtotal = 0;
                    foreach (var x in cartItems)
                    {

                        subtotal += x.Quantity * (int)GetProductById($"{x.ProductId}").Price;
                    }
                    var displayTotal = new CartUser() { Cart = cart, CartItem = cartItems, Products = product, SubTotal = (short)subtotal };
                    return View(displayTotal);
                }
            }
            else
            {
                return Redirect("/Shop");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(int num)
        {
            var currentUserId = HttpContext.Session.GetString("UserId");
            var cart = _db.Cart.Where(x => x.UserId.Equals(currentUserId)).FirstOrDefault();
            var cartItems = _db.CartItem.Where(x => x.ShoppingCartId.Equals(cart.CartId)).ToList();
            var number_generator = new Random();
            var OrderID = number_generator.Next(100000, 999999);
            foreach (var x in cartItems)
            {
                var addOrder = new OrderDetails { ProductId = x.ProductId, Quantity = x.Quantity, UnitPrice = x.ProductPrice, CustomerID = cart.UserId, Discount = 0, TransactionId = OrderID };
                _db.OrderDetails.Add(addOrder);
                _db.SaveChanges();
                _db.Entry<OrderDetails>(addOrder).State = EntityState.Detached;
            }

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./database.db";
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            var selectCmd = connection.CreateCommand();
            //selectCmd.CommandText = "select * from products where title = '100' or '1' = '1'";
            selectCmd.CommandText = $"delete from CartItem where ShoppingCartId = {cart.CartId}";
            var result = selectCmd.ExecuteNonQuery();

            ViewBag.CheckoutOk = "Successful";
            return View();
        }

    }
}
