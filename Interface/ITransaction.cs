using CartManagmentSystem.Models;

namespace CartManagmentSystem.Interface
{
    /// <summary>
    /// Interface for managing transactions.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Adds an item to the cart.
        /// </summary>
        /// <param name="logId">The ID of the log entry.</param>
        /// <param name="data">The cart item data to be added.</param>
        /// <param name="cartContainer">Output parameter containing the updated cart container.</param>
        /// <param name="savedMessage">Output parameter containing the status message.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        bool AddToCart(int logId, CartItemData data, out CartContainer cartContainer, out string savedMessage);

        /// <summary>
        /// Removes an item from the cart.
        /// </summary>
        /// <param name="logId">The ID of the log entry.</param>
        /// <param name="data">The cart item data to be removed.</param>
        /// <param name="cartContainer">Output parameter containing the updated cart container.</param>
        /// <param name="savedMessage">Output parameter containing the status message.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        bool RemoveFromCart(int logId, RemoveItem data, out CartContainer cartContainer, out string savedMessage);

        /// <summary>
        /// Retrieves all items in the cart based on specified filters.
        /// </summary>
        /// <param name="logId">The ID of the log entry.</param>
        /// <param name="data">The filter criteria for retrieving cart items.</param>
        /// <param name="itemFilterData">Output parameter containing the filtered cart items.</param>
        /// <param name="savedMessage">Output parameter containing the status message.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        bool AllCartItems(int logId, ItemFilter data, out List<ItemFilterData> itemFilterData, out string savedMessage);

        /// <summary>
        /// Retrieves a specific item in the cart.
        /// </summary>
        /// <param name="logId">The ID of the log entry.</param>
        /// <param name="cartItemId">The ID of the cart item to retrieve.</param>
        /// <param name="itemFilterData">Output parameter containing the retrieved cart item.</param>
        /// <param name="savedMessage">Output parameter containing the status message.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        bool GetCartItem(int logId, int cartItemId, out ItemFilterData? itemFilterData, out string savedMessage);
    }
}
