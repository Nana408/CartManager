using CartManagmentSystem.Entities;
using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace CartManagmentSystem.Repositories
{
    // TransactionsRepository class is responsible for managing transactions related to the e-commerce cart.
    public class TransactionsRepository : ITransaction
    {
        // Context for accessing the database
        private readonly CartManagementSystemContext _context;

        // Constructor to initialize the context
        public TransactionsRepository(CartManagementSystemContext context)
        {
            _context = context;
        }

        // Flag to track whether the object has been disposed
        private bool disposed = false;

        // Method to dispose the context
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

        // IDisposable implementation to properly dispose the context
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /*
 AddToCart method is responsible for adding items to the cart.
 Parameters:
 - logId: ID for logging purposes
 - data: CartItems object containing the data to be added to the cart
 - cartContainer: Output parameter for the updated cart container
 - savedMessage: Output parameter for the message indicating the result of the operation
 Returns:
 - Boolean indicating whether the operation was successful
 */
        public bool AddToCart(int logId, CartItemData data, out CartContainer cartContainer, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            cartContainer = new CartContainer();
            try
            {

                Cart cart1 = _context.Carts.FirstOrDefault(x => x.UserId == data.UserId);

                if (cart1 != null)
                {
                    // Update the last updated details of the cart
                    cart1.LastUpdatedBy = data.UserId.ToString();
                    cart1.LastUpdatedDate = DateTime.UtcNow;

                    // Mark the cart as modified in the context
                    _context.Entry(cart1).State = EntityState.Modified;
                    _context.SaveChanges();

                    //data.CartId = cart1.CartId;
                    // Update the cart items
                    if (UpdateCartItems(logId, cart1.CartId, data))
                    {
                        savedMessage = StaticVariables.SUCCESSMESSAGE;
                        // Retrieve the updated cart items
                        cartContainer = new CartContainer
                        {
                            CartId = cart1.CartId,
                            items = GetCartItems(logId, cart1.CartId)
                        };

                        worked = true;
                    }
                }
                else
                {
                    // Create a new cart if it doesn't exist
                    Cart cart = new()
                    {
                        UserId = data.UserId,
                        CreationBy = data.UserId.ToString(),
                        CreationDate = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                   

                    // Add cart items to the newly created cart
                    if (AddCartItems(logId, cart.CartId, data))
                    {
                        savedMessage = StaticVariables.SUCCESSMESSAGE;
                        // Retrieve the cart items
                        cartContainer = new CartContainer
                        {
                            CartId = cart.CartId,
                            items = GetCartItems(logId, cart.CartId)
                        };
                        worked = true;
                    }
                }
                /*
                // Check if the cart already exists
                if (data.CartId > 0)
                {
                    // Retrieve the cart from the database
                    Cart cart1 = _context.Carts.FirstOrDefault(x => x.CartId == data.CartId && x.UserId == data.UserId);

                    if (cart1 != null)
                    {
                        // Update the last updated details of the cart
                        cart1.LastUpdatedBy = data.UserId.ToString();
                        cart1.LastUpdatedDate = DateTime.UtcNow;

                        // Mark the cart as modified in the context
                        _context.Entry(cart1).State = EntityState.Modified;
                        _context.SaveChanges();

                        // Update the cart items
                        if (UpdateCartItems(logId, data))
                        {
                            savedMessage = StaticVariables.SUCCESSMESSAGE;
                            // Retrieve the updated cart items
                            cartContainer = new CartContainer
                            {
                                CartId = data.CartId,
                                items = GetCartItems(logId, data.CartId)
                            };

                            worked = true;
                        }
                    }
                }
                else
                {
                    // Create a new cart if it doesn't exist
                    Cart cart = new()
                    {
                        UserId = data.UserId,
                        CreationBy = data.UserId.ToString(),
                        CreationDate = DateTime.UtcNow
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    data.CartId = cart.CartId;

                    // Add cart items to the newly created cart
                    if (AddCartItems(logId, data))
                    {
                        savedMessage = StaticVariables.SUCCESSMESSAGE;
                        // Retrieve the cart items
                        cartContainer = new CartContainer
                        {
                            CartId = data.CartId,
                            items = GetCartItems(logId, data.CartId)
                        };
                        worked = true;
                    }
                }
                */
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }

        /*
 AddCartItems method is responsible for adding a new cart item to the database.
 Parameters:
 - logId: ID for logging purposes
 - data: CartItems object containing the data of the item to be added to the cart
 Returns:
 - Boolean indicating whether the operation was successful
 */
        public bool AddCartItems(int logId, int cartId, CartItemData data)
        {
            bool worked = true;
            try
            {
                // Create a new cart item object
                CartItem cartItem = new CartItem
                {
                    CartId = cartId,
                    CreationBy = data.UserId.ToString(),
                    CreationDate = DateTime.UtcNow,
                    ProductId = data.ProductId,
                    Quantity = data.Quantity,
                    Price = GetPrice(logId, data.Quantity, data.ProductId)
                };

                // Add the cart item to the context and save changes to the database
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                worked = false;
            }
            return worked;
        }

        /*
        GetPrice method retrieves the price of a product based on its quantity and product ID.
        Parameters:
        - logId: ID for logging purposes
        - quanty: Quantity of the product
        - productId: ID of the product
        Returns:
        - Decimal value indicating the price of the product
        */
        public decimal GetPrice(int logId, int quanty, int productId)
        {
            decimal price = 0M;
            try
            {
                // Retrieve the product from the database
                Product product = _context.Products.Find(productId);

                if (product != null)
                {
                    // Calculate the price based on the quantity and price of the product
                    price = product.Price * quanty;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(new { quanty, productId }), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }
            return price;
        }

        /*
        UpdateCartItems method is responsible for updating the quantity and price of a cart item in the database.
        Parameters:
        - logId: ID for logging purposes
        - data: CartItems object containing the updated data of the cart item
        Returns:
        - Boolean indicating whether the operation was successful
        */
        public bool UpdateCartItems(int logId, int cartId, CartItemData data)
        {
            bool worked = true;
            try
            {
                // Find the cart item in the database
                CartItem cartItem = _context.CartItems.FirstOrDefault(x => x.CartId == cartId && x.ProductId == data.ProductId);

                if (cartItem != null)
                {
                    // Update the quantity and price of the cart item
                    cartItem.Quantity += data.Quantity;
                    cartItem.Price = GetPrice(logId, cartItem.Quantity, data.ProductId);

                    // Mark the cart item as modified in the context and save changes to the database
                    _context.Entry(cartItem).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    // Create a new cart item if it doesn't exist
                    CartItem cartItem1 = new CartItem
                    {
                        CartId = cartId,
                        CreationBy = data.UserId.ToString(),
                        CreationDate = DateTime.UtcNow,
                        ProductId = data.ProductId,
                        Quantity = data.Quantity,
                        Price = GetPrice(logId, data.Quantity, data.ProductId)
                    };

                    // Add the new cart item to the context and save changes to the database
                    _context.CartItems.Add(cartItem1);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                worked = false;
            }
            return worked;
        }
        /*
        GetCartItems method retrieves all cart items associated with a given cart ID from the database.
        Parameters:
        - logId: ID for logging purposes
        - cartId: ID of the cart to retrieve items from
        Returns:
        - List of Items containing the cart items retrieved from the database
        */
        public List<Items> GetCartItems(int logId, int cartId)
        {
            List<Items> cartItems = new List<Items>();
            try
            {
                // Retrieve cart items from the database including associated product information
                cartItems = _context.CartItems
                    .Include(x => x.Product)
                    .Where(x => x.CartId == cartId)
                    .Select(x => new Items
                    {
                        Name = x.Product.ProductName,
                        Quantity = x.Quantity,
                        Price = x.Price
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(cartId), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }

            return cartItems;
        }

        /*
        RemoveFromCart method removes a specific item from the cart in the database.
        Parameters:
        - logId: ID for logging purposes
        - data: RemoveItem object containing the data of the item to be removed from the cart
        - cartContainer: Out parameter containing the updated cart information after item removal
        - savedMessage: Out parameter indicating the result message of the operation
        Returns:
        - Boolean indicating whether the operation was successful
        */
        public bool RemoveFromCart(int logId, RemoveItem data, out CartContainer cartContainer, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            cartContainer = new CartContainer();
            try
            {
                // Find the cart item to remove from the database
                CartItem cartItem = _context.CartItems.FirstOrDefault(x => x.Cart.User.UserId == data.UserId && x.ProductId == data.ProductId);

                if (cartItem != null)
                {
                    // Remove the cart item from the context and save changes to the database
                    _context.Remove(cartItem);
                    _context.SaveChanges();

                    // Update the cart container with the remaining items
                    cartContainer = new CartContainer
                    {
                        CartId = (int)cartItem.CartId,
                        items = GetCartItems(logId, (int)cartItem.CartId)
                    };

                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }

        /*
AllCartItems method retrieves all cart items based on specified filters.
Parameters:
- logId: ID for logging purposes
- data: ItemFilter object containing filter parameters
- itemFilterData: List of ItemFilterData representing the filtered cart items
- savedMessage: Message indicating the outcome of the operation
Returns:
- Boolean indicating whether the operation was successful
*/
        public bool AllCartItems(int logId, ItemFilter data, out List<ItemFilterData> itemFilterData, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            itemFilterData = new List<ItemFilterData>();
            try
            {
                // Initialize a queryable collection of cart items
                IQueryable<CartItem> query = _context.CartItems;
                DateTime? startDate = null;
                DateTime? endDate = null;

                // Parse the 'From' and 'To' date filters
                if (DateTime.TryParse(data.From, out DateTime start))
                    startDate = start;

                if (DateTime.TryParse(data.To, out DateTime end))
                    endDate = end;

                // Apply date filters to the query
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

                // Apply product ID filter to the query
                if (data.ProductId != null && data.ProductId > 0)
                {
                    query = query.Where(cartItem => cartItem.Product.ProductId == data.ProductId);
                }

                // Apply phone number filter to the query
                if (!string.IsNullOrEmpty(data.PhoneNumber))
                {
                    query = query.Where(cartItem => cartItem.Cart.User.Phonenumber == data.PhoneNumber);
                }

                // Apply quantity filter to the query
                if (data.Quantity != null && data.Quantity > 0)
                {
                    query = query.Where(cartItem => cartItem.Quantity == data.Quantity);
                }

                // Select and map the filtered cart items to ItemFilterData objects
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

                // Set operation success and update saved message
                worked = true;
                savedMessage = StaticVariables.SUCCESSMESSAGE;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(data), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }

        /*
GetCartItem method retrieves a specific cart item based on its ID.
Parameters:
- logId: ID for logging purposes
- cartItemId: ID of the cart item to retrieve
- itemFilterData: ItemFilterData representing the retrieved cart item
- savedMessage: Message indicating the outcome of the operation
Returns:
- Boolean indicating whether the operation was successful
*/
        public bool GetCartItem(int logId, int cartItemId, out ItemFilterData? itemFilterData, out string savedMessage)
        {
            bool worked = false;
            savedMessage = StaticVariables.FAILEDMESSAGE;
            itemFilterData = new ItemFilterData();
            try
            {
                // Query to retrieve the specified cart item and join related entities
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

                // If the item is found, set operation success and update saved message
                if (itemFilterData != null)
                {
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(cartItemId), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
            }

            return worked;
        }
    }
}
