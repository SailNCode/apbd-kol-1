using Tutorial9.Model_DTOs;

namespace Tutorial9.Services;

public interface IWarehouseService
{
    Task<int> FulfillOrderRequest(FulfillOrderRequest fulfillRequest);
    
}