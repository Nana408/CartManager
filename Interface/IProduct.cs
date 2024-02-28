using CartManagmentSystem.Models;

namespace CartManagmentSystem.Interface
{
    public interface IProduct : IDisposable
    {
        bool GetProducts(int logId, out List<ProductData> data, out string savedMessage);
    }
}
