using CartManagmentSystem.Models;

namespace CartManagmentSystem.Interface
{
    /// <summary>
    /// Interface for managing products.
    /// </summary>
    public interface IProduct : IDisposable
    {
        /// <summary>
        /// Retrieves a list of products.
        /// </summary>
        /// <param name="logId">The ID of the log entry.</param>
        /// <param name="data">Output parameter containing the list of product data.</param>
        /// <param name="savedMessage">Output parameter containing the status message.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        bool GetProducts(int logId, out List<ProductData> data, out string savedMessage);
    }
}
