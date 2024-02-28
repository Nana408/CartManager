using CartManagmentSystem.Entities;
using CartManagmentSystem.Interface;
using CartManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace CartManagmentSystem.Repositories
{
    public class ProductsPrepository : IProduct
    {
        private readonly CartManagementSystemContext _context;

        public ProductsPrepository(CartManagementSystemContext context)
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

        public bool GetProducts(int logId,out List<ProductData> data, out string savedMessage)
        {
            bool worked = false;
            data = new List<ProductData>();
            savedMessage = StaticVariables.FAILEDMESSAGE;
            try
            {
                data = _context.Products.Select(x => new ProductData
                {
                    Id = x.ProductId,
                    Description = x.Description,
                    Name = x.ProductName,
                    Quantity = x.Quantity,
                    Price = x.Price,

                }).ToList();

                if (data.Any())
                {
                    worked = true;
                    savedMessage = StaticVariables.SUCCESSMESSAGE;
                }
            }
            catch (Exception ex)
            {
              
                savedMessage = StaticVariables.EXCEPTIONMESSAGE;
                _ = Task.Factory.StartNew(() => UserFunctions.WriteLog(logId, JsonConvert.SerializeObject(""), ex.Message +" ||" +ex.StackTrace , StaticVariables.API, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name)));
            }

            return worked;
        }
    }
}
