using CartManagmentSystem.Entities;
using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace CartManagmentSystem.Repositories
{
    // ProductsPrepository class is responsible for interacting with the database to retrieve product data.
    public class ProductsPrepository : IProduct
    {
        // Context for accessing the database
        private readonly CartManagementSystemContext _context;

        // Constructor to initialize the context
        public ProductsPrepository(CartManagementSystemContext context)
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

        // Method to retrieve product data from the database
        // Parameters:
        // - logId: ID for logging purposes
        // - data: Output parameter for the retrieved product data
        // - savedMessage: Output parameter for the message indicating the result of the operation
        // Returns:
        // - Boolean indicating whether the operation was successful
        public bool GetProducts(int logId, out List<ProductData> data, out string savedMessage)
        {
            bool worked = false;
            data = new List<ProductData>();
            savedMessage = StaticVariables.FAILEDMESSAGE;
            try
            {
                // Retrieve product data from the database and map it to ProductData objects
                data = _context.Products.Select(x => new ProductData
                {
                    Id = x.ProductId,
                    Description = x.Description,
                    Name = x.ProductName,
                    Quantity = x.Quantity,
                    Price = x.Price,

                }).ToList();

                // If data is retrieved successfully, set worked to true and update the savedMessage
                if (data.Any())
                {
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the error and update the savedMessage
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(""), ex.Message + " ||" + ex.StackTrace, StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }

            return worked;
        }
    }
}
