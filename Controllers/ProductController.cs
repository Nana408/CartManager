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
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepository;

        public ProductController()
        {
            _productRepository = new ProductsPrepository(new Entities.CartManagementSystemContext());

        }


        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            string companyName = Request.Headers[StaticVariables.COMPANYNAME];
            int CompanyID = int.Parse(Request.Headers[StaticVariables.COMPANYID]);
            string actionName = ControllerContext.RouteData.Values["action"].ToString();
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress + "/" + Request.Headers[StaticVariables.USERAGENT];
            var requestBody = JsonConvert.SerializeObject("", new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            int logId = UserFunctions.InsertLog(ipAddress, actionName, companyName, requestBody);
            try
            {
                bool worked = _productRepository.GetProducts(logId,out List<ProductData> products, out string savedMessage);

                if (worked)
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.SUCCESSSTATUS, CompanyID, savedMessage, companyName));
                    var success = new { Status = StaticVariables.SUCCESSSTATUS, Message = savedMessage, Products = products };
                    return Ok(success);
                }
                else
                {
                    Task.Factory.StartNew(() => UserFunctions.UpdateLogs(logId, StaticVariables.FAILEDSTATUS, CompanyID, savedMessage, companyName));
                    var error = new { Status = StaticVariables.FAILEDSTATUS, Message = savedMessage, Products = products };
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
