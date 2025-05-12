using Tutorial9.Model_DTOs;

namespace Tutorial9.Repositories;

public interface IRepository: ITransactional
{
    //ToRemove
    Task FulfillOrderAtCurrentDate(int orderId);
    //ToRemove
    Task<int> AddProduct_WarehouseRecord(int warehouseId, int productId, int orderId, int amount, decimal price);
    //ToRemove
    Task<bool> IsVisitPresent(int visitId);
    //ToRemove
    Task<ClientDTO> GetClient(int visitClientId);
    //ToRemove
    Task<List<VisitServiceDTO>> GetVisits(int visitId);
}