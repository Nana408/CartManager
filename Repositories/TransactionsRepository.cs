using CartManagmentSystem.Entities;
using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace CartManagmentSystem.Repositories
{
    public class TransactionsRepository : ITransaction
    {
        private readonly CartManagementSystemContext _context;

        public TransactionsRepository(CartManagementSystemContext context)
        {
            _context = context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public bool AddToCart(int logId,CartItems data, out CartContainer cartContainer, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            cartContainer = new CartContainer();
            try
            {
                if (data.CartId > 0)
                {
                    Cart cart1 = _context.Carts.FirstOrDefault(x => x.CartId == data.CartId && x.UserId == data.UserId);

                    if (cart1 != null)
                    {
                        cart1.LastUpdatedBy = data.UserId.ToString();
                        cart1.LastUpdatedDate = DateTime.UtcNow;

                        _context.Entry(cart1).State = EntityState.Modified;
                        _context.SaveChanges();

                        if (UpdateCartItems(logId,data))
                        {
                            savedMessage = StaticVariables.SUCCESSMESSAGE;
                            cartContainer = new()
                            {
                                CartId = data.CartId,
                                items = GetCartItems(logId,data.CartId)
                            };

                            worked = true;
                        }
                    }
                }
                else
                {
                    Cart cart = new()
                    {
                        UserId = data.UserId,
                        CreationBy = data.UserId.ToString(),
                        CreationDate = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    data.CartId = cart.CartId;

                    if (AddCartItems(logId,data))
                    {
                        savedMessage = StaticVariables.SUCCESSMESSAGE;
                        cartContainer = new()
                        {
                            CartId = data.CartId,
                            items = GetCartItems(logId,data.CartId)
                        };
                        worked = true;
                    }
                }

            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }

        public bool AddCartItems(int logId,CartItems data)
        {
            bool worked = true;
            try
            {
                CartItem cartItem = new()
                {
                    CartId = data.CartId,
                    CreationBy = data.UserId.ToString(),
                    CreationDate = DateTime.UtcNow,
                    ProductId = data.ProductId,
                    Quantity = data.Quantity,
                    Price = GetPrice(logId, data.Quantity, data.ProductId)
                };

                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
                worked = true;
            }
            catch (Exception ex)
            {

                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }
            return worked;
        }

        public  decimal GetPrice(int logId,int quanty, int productId)
        {
            decimal price = 0M;
            try
            {
                Product product = _context.Products.Find(productId);

                if (product != null)
                {
                    price = product.Price * quanty;
                }

            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(new {quanty,productId}), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }
            return price;
        }
        public bool UpdateCartItems(int logId,CartItems data)
        {
            bool worked = true;
            try
            {
                CartItem cartItem = _context.CartItems.FirstOrDefault(x => x.CartId == data.CartId && x.ProductId == data.ProductId);

                if (cartItem != null)
                {
                    cartItem.Quantity += data.Quantity;
                    cartItem.Price = GetPrice( logId, cartItem.Quantity, data.ProductId);

                    _context.Entry(cartItem).State = EntityState.Modified;
                    _context.SaveChanges();
                    worked = true;
                }
                else
                {
                    CartItem cartItem1 = new()
                    {
                        CartId = data.CartId,
                        CreationBy = data.UserId.ToString(),
                        CreationDate = DateTime.UtcNow,
                        ProductId = data.ProductId,
                        Quantity = data.Quantity,
                        Price = GetPrice(logId, data.Quantity, data.ProductId)
                    };

                    _context.CartItems.Add(cartItem1);
                    _context.SaveChanges();
                    worked = true;
                }


            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));

            }
            return worked;
        }

        public List<Items> GetCartItems(int logId, int cartId)
        {
            List<Items> cartItems = new();
            try
            {
                cartItems = _context.CartItems.Include(x => x.Product).Where(x => x.CartId == cartId).Select(x => new Items
                {
                    Name = x.Product.ProductName,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList();
            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(cartId), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));

            }

            return cartItems;
        }

        public bool RemoveFromCart(int logId, RemoveItem data, out CartContainer cartContainer, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            cartContainer = new CartContainer();
            try
            {
                CartItem cartItem = _context.CartItems.FirstOrDefault(x => x.CartId == data.CartId && x.ProductId == data.ProductId);

                if (cartItem != null)
                {
                    _context.Remove(cartItem);
                    _context.SaveChanges();

                    cartContainer = new()
                    {
                        CartId = data.CartId,
                        items = GetCartItems(logId,data.CartId)
                    };
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }

        public bool AllCartItems(int logId,ItemFilter data, out List<ItemFilterData> itemFilterData, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            itemFilterData = new List<ItemFilterData>();
            try
            {
                IQueryable<CartItem> query = _context.CartItems;
                DateTime? startDate = null;
                DateTime? endDate = null;

               
                if (DateTime.TryParse(data.From, out DateTime start))
                    startDate = start;

                if (DateTime.TryParse(data.To, out DateTime end))
                    endDate = end;

               
                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(cartItem => cartItem.Cart.CreationDate >= startDate && cartItem.Cart.CreationDate <= endDate);
                }
                else if (startDate.HasValue)
                {
                    
                    query = query.Where(cartItem => cartItem.Cart.CreationDate >= startDate.Value);
                }
                else if (endDate.HasValue)
                {
              
                    query = query.Where(cartItem => cartItem.Cart.CreationDate <= endDate.Value);
                }

                if (data.ProductId != null && data.ProductId > 0)
                {
                    query = query.Where(cartItem => cartItem.Product.ProductId == data.ProductId);
                }

                if (!string.IsNullOrEmpty(data.PhoneNumber))
                {
                    query = query.Where(cartItem => cartItem.Cart.User.Phonenumber == data.PhoneNumber);
                }

                if (data.Quantity != null && data.Quantity > 0)
                {
                    query = query.Where(cartItem => cartItem.Quantity == data.Quantity);
                }

               

                itemFilterData = query.Select(cartItem => new ItemFilterData
                {
                    CartItemId = cartItem.CartItemId,
                    PhoneNumber = cartItem.Cart.User.Phonenumber,
                    CartId = cartItem.CartId,
                    Name = cartItem.Cart.User.Username,
                    Product = cartItem.Product.ProductName,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    CreatedDate = cartItem.CreationDate
                }).ToList();

                worked = true;
                savedMessage = StaticVariables.SUCCESSMESSAGE;

            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }

        public bool GetCartItem(int logId, int cartItemId, out ItemFilterData? itemFilterData, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            itemFilterData = new ItemFilterData();
            try
            {
                itemFilterData = _context.CartItems
               .Where(item => item.CartItemId == cartItemId)
               .Join(
                   _context.Carts,
                   cartItem => cartItem.CartId,
                   cart => cart.CartId,
                   (cartItem, cart) => new { cartItem, cart }
               )
               .Join(
                   _context.Users,
                   combined => combined.cart.UserId,
                   user => user.UserId,
                   (combined, user) => new { combined.cartItem, combined.cart, user }
               )
               .Join(
                   _context.Products,
                   combined => combined.cartItem.ProductId,
                   product => product.ProductId,
                   (combined, product) => new ItemFilterData
                   {
                       CartItemId = combined.cartItem.CartItemId,
                       Name = combined.user.Username,
                       PhoneNumber = combined.user.Phonenumber,
                       Product = product.ProductName,
                       CartId = combined.cart.CartId,
                       Quantity = combined.cartItem.Quantity,
                       Price = combined.cartItem.Price,
                       CreatedDate = combined.cartItem.CreationDate
                   }
               )
               .FirstOrDefault();

                if (itemFilterData != null)
                {
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }


            }
            catch (Exception ex)
            {
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(cartItemId), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }
    }
}
