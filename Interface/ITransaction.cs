using CartManagmentSystem.Models;

namespace CartManagmentSystem.Interface
{
     interface ITransaction : IDisposable
    {
        bool AddToCart(int logId, CartItems data, out CartContainer cartContainer, out string savedMessage);

        bool RemoveFromCart(int  logId,    RemoveItem data, out CartContainer cartContainer, out string savedMessage);

        bool AllCartItems(int logId, ItemFilter data, out List<ItemFilterData> itemFilterData, out string savedMessage);

        bool GetCartItem(int logId, int cartItemId, out ItemFilterData? itemFilterData, out string savedMessage);
    }
}
