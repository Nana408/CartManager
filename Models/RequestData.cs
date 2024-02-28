namespace CartManagmentSystem.Models
{
    public class CartItems
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartContainer
    {
        public int CartId { get; set; }
        public List< Items> items { get; set; }

    }

    public class Items
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

    }

    public class RemoveItem
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    
    }

    public class ItemFilter
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public int? ProductId { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Quantity { get; set; }
    }

    public class ItemFilterData
    {
        public int CartItemId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Product { get; set; }
        public int? CartId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }     
       
    }
}
