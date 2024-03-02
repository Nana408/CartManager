using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using CartManagmentSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;

namespace CartManagmentSystem.Controllers
{
    /// <summary>
    /// Controller for managing the user's cart.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ITransaction _transactionRepository;

        /// <summary>
        /// Constructor for CartController class.
        /// </summary>
        public CartController()
        {
            _transactionRepository = new TransactionsRepository(new Entities.CartManagementSystemContext());
        }

        /// <summary>
        /// Adds an item to the cart.
        /// </summary>
        /// <param name="data">The cart item data to add.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpPost("AddCartItem")]
        public IActionResult AddToCart(CartItemData data)
        {
            // Retrieving request headers and other necessary information
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
                // Attempting to add the item to the cart
                bool worked = _transactionRepository.AddToCart(logId, data, out CartContainer cartContainer, out string savedMessage);

                if (worked)
                {
                    // If addition was successful, update logs and return the cart
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, Cart = cartContainer };
                    return Ok(success);
                }
                else
                {
                    // If addition failed, update logs and return an error
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, Cart = cartContainer };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurred during addition, log the error and return an exception message
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);
            }
        }

        /// <summary>
        /// Removes an item from the cart.
        /// </summary>
        /// <param name="data">The item data to remove from the cart.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpDelete("RemoveCartItem")]
        public IActionResult RemoveFromCart(RemoveItem data)
        {
            // Retrieving request headers and other necessary information
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
                // Attempting to remove the item from the cart
                bool worked = _transactionRepository.RemoveFromCart(logId, data, out CartContainer cartContainer, out string savedMessage);

                if (worked)
                {
                    // If removal was successful, update logs and return the cart
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, Cart = cartContainer };
                    return Ok(success);
                }
                else
                {
                    // If removal failed, update logs and return an error
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, Cart = cartContainer };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurred during removal, log the error and return an exception message
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);
            }
        }

        /// <summary>
        /// Filters cart items based on specified parameters.
        /// </summary>
        /// <param name="ProductId">The ID of the product to filter by.</param>
        /// <param name="Quantity">The quantity to filter by.</param>
        /// <param name="PhoneNumber">The phone number to filter by.</param>
        /// <param name="From">The start date range to filter by.</param>
        /// <param name="To">The end date range to filter by.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("FilterCartItem")]
        public IActionResult FilterCartItem(int? ProductId = null, int? Quantity = null, string? PhoneNumber = null, string? From = null, string? To = null)
        {
            // Creating item filter data based on parameters
            ItemFilter data = new() { From = From, To = To, PhoneNumber = PhoneNumber, ProductId = ProductId, Quantity = Quantity };
            // Retrieving request headers and other necessary information
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
                // Attempting to filter cart items based on the provided parameters
                bool worked = _transactionRepository.AllCartItems(logId, data, out List<ItemFilterData> itemFilterData, out string savedMessage);

                if (worked)
                {
                    // If filtering was successful, update logs and return the filtered cart items
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, CartItems = itemFilterData };
                    return Ok(success);
                }
                else
                {
                    // If filtering failed, update logs and return an error
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, CartItems = itemFilterData };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurred during filtering, log the error and return an exception message
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);
            }
        }

        /// <summary>
        /// Retrieves a specific cart item.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to retrieve.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("GetCartItem/{cartItemId}")]
        public IActionResult GetCartItem(int cartItemId)
        {
            // Retrieving request headers and other necessary information
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(cartItemId, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
                // Attempting to retrieve the specific cart item
                bool worked = _transactionRepository.GetCartItem(logId, cartItemId, out ItemFilterData? itemFilterData, out string savedMessage);

                if (worked)
                {
                    // If retrieval was successful, update logs and return the cart item
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, CartItem = itemFilterData };
                    return Ok(success);
                }
                else
                {
                    // If retrieval failed, update logs and return an error
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, CartItem = itemFilterData };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurred during retrieval, log the error and return an exception message
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);
            }
        }
    }
}
