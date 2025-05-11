using Tutorial9.Model_DTOs;

namespace Tutorial9.Repositories;

public interface IWarehouseRepository: ITransactional
{
    Task<bool> IsProductPresent(int productId);
    Task<int> GetOrderId(int productId, int amount);
    Task FulfillOrderAtCurrentDate(int orderId);
    Task<Product> GetProduct(int productId);
    Task<int> AddProduct_WarehouseRecord(int warehouseId, int productId, int orderId, int amount, decimal price);
}