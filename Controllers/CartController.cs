using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using CartManagmentSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;

namespace CartManagmentSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private ITransaction _transactionRepository;


        public CartController()
        {
            _transactionRepository = new TransactionsRepository(new Entities.CartManagementSystemContext());

        }


        [HttpPost("AddCartItem")]
        public IActionResult AddToCart(CartItems data)
        {
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
            

                bool worked = _transactionRepository.AddToCart(logId, data, out CartContainer cartContainer, out string savedMessage);

                if (worked)
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, Cart = cartContainer };
                    return Ok(success);
                }
                else
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, Cart = cartContainer };
                    return NotFound(error);
                }


            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500,error);

            }
        }

        [HttpDelete("RemoveCartItem")]
        public IActionResult RemoveFromCart(RemoveItem data)
        {
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);
            try
            {
               

                bool worked = _transactionRepository.RemoveFromCart(logId, data, out CartContainer cartContainer, out string savedMessage);

                if (worked)
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, Cart = cartContainer };
                    return Ok(success);
                }
                else
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, Cart = cartContainer };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);

            }
        }

      
        [HttpGet("FilterCartItem")]
        public IActionResult FilterCartItem(int? ProductId = null, int? Quantity = null, string? PhoneNumber = null, string? From = null, string? To = null)
        {
            ItemFilter data = new() { From = From, To = To, PhoneNumber = PhoneNumber, ProductId = ProductId, Quantity = Quantity };
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
                
                bool worked = _transactionRepository.AllCartItems(logId, data, out List<ItemFilterData> itemFilterData, out string savedMessage);

                if (worked)
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, CartItems = itemFilterData };
                    return Ok(success);
                }
                else
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, CartItems = itemFilterData };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);

            }
        }

        [HttpGet("GetCartItem/{cartItemId}")]
        public IActionResult GetCartItem(int cartItemId)
        {
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject(cartItemId, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);

            try
            {
               

                bool worked = _transactionRepository.GetCartItem(logId,cartItemId, out ItemFilterData? itemFilterData, out string savedMessage);

                if (worked)
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, CartItem = itemFilterData };
                    return Ok(success);
                }
                else
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, CartItem = itemFilterData };
                    return NotFound(error);
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.EXCEPTIONSTATUS, CompanyID, ex.Message + "||" + ex.StackTrace, companyName));
                var error = new { Status = StaticVariables.EXCEPTIONSTATUS, Message = StaticVariables.EXCEPTIONMESSAGE };
                return StatusCode(500, error);

            }
        }

    }
}
